using System.Text.Json;
using CVHosting.Shared.Models.Entities.ApplicantProfile;
using CVRecognizingService.Application.FluentValidation;
using CVRecognizingService.Application.Helpers.AiPdfComanion;
using CVRecognizingService.Application.Helpers.PDFConverter;
using CVRecognizingService.Application.Helpers.PDFRecognizing;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.Enums;
using CVRecognizingService.Domain.Exeptions;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using DotnetGeminiSDK.Client.Interfaces;
using Events_Web_application.Application.Services.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.UseCases.Commands.Documents;

public class CreateDocumentCommandHandler
    : IRequestHandler<CreateDocumentCommand, CreateDocumentResponse>
{
    private readonly ILogger<CreateDocumentCommandHandler> _logger;

    private readonly DocumentRepository _documentRepository;
    private readonly ProcessingStatusRepository _processingStatusRepository;
    private readonly ProcessedDataRepository _processedDataRepository;
    private readonly ProcessingLogRepository _processingLogRepository;

    private readonly GeminiAITextChat _chat;
    private readonly FileValidator _fileValidator;

    private delegate Task Update(CancellationToken cancellationToken, DocumentState state);
    private event Update? OnUpdate;
    private Document? _document;
    private ProcessingStatus? _docstatus;

    public CreateDocumentCommandHandler(
        IGeminiClient geminiClient,
        IValidator<IFormFile> fileValidator,
        ILogger<CreateDocumentCommandHandler> logger,
        IRepository<Document> documentRepository,
        IRepository<ProcessingStatus> processingStatusRepository,
        IRepository<ProcessedData> processedDataRepository,
        IRepository<ProcessingLog> processedLogRepository)
    {
        _logger = logger;
        _chat = new GeminiAITextChat(geminiClient);
        _fileValidator = (FileValidator)fileValidator;
        _documentRepository = (DocumentRepository)documentRepository;
        _processingStatusRepository = (ProcessingStatusRepository)processingStatusRepository;
        _processedDataRepository = (ProcessedDataRepository)processedDataRepository;
        _processingLogRepository = (ProcessingLogRepository)processedLogRepository;
        OnUpdate += UpdateDocumentState;
    }

    public async Task<CreateDocumentResponse> Handle(
        CreateDocumentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"{request.File.FileName} started recognizing");
            var result = await AddDocument(request.File, request.UserId, cancellationToken);
            return result;
        }
        catch (ServiceException ex)
        {
            _logger.LogInformation($"{request.File.FileName} finished recognizing with error: {ex.Value}");
            throw;
        }
    }
    private async Task InvokeOnUpdateAsync(CancellationToken cancellationToken, DocumentState state)
    {
        if (OnUpdate == null) return;
        var invocationList = OnUpdate.GetInvocationList();
        foreach (Update handler in invocationList)
        {
            await handler(cancellationToken, state);
        }
    }

    private async Task<long> AddFileToDataBase(Document document, CancellationToken cancellationToken)
    {
        if (document == null) throw new NullObjectException(nameof(document));

        var result = await _documentRepository.AddAsync(document, cancellationToken);

        _logger.LogInformation($"Document from {document.FileName} was recognized and added to database");

        return result;
    }

    private async Task<string> GetFormattedText(string nonFormatedText, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(nonFormatedText)) throw new ArgumentException(nameof(nonFormatedText));

        var result = await _chat.GetFormatedText(nonFormatedText, cancellationToken);

        await InvokeOnUpdateAsync(cancellationToken, DocumentState.Processing);

        _logger.LogInformation($"Text|\n {nonFormatedText} \n formatted {result.Candidates[0].Content.Parts[0].Text}");

        return result.Candidates[0].Content.Parts[0].Text;
    }

    private async Task<string> RecognizeText(IFormFile file, CancellationToken cancellationToken)
    {
        var recognizedText = new PDFRecognizer(await file.GetBytesAsync(cancellationToken));

        await InvokeOnUpdateAsync(cancellationToken, DocumentState.Processing);

        _logger.LogInformation($"Text from {file} recognized {recognizedText.RecognizedText}");

        var isFileCV = await _chat.IsCV(recognizedText.RecognizedText, cancellationToken);
        if (isFileCV.Candidates[0].Content.Parts[0].Text.Contains("false", StringComparison.OrdinalIgnoreCase)
            || recognizedText.RecognizedText.Length == 0)
        {
            throw new ServiceException(nameof(this.RecognizeText), recognizedText.RecognizedText, "File is not Cover Leter or Resume");
        }
        return recognizedText.RecognizedText;
    }

    private async Task UpdateDocumentState(CancellationToken cancellationToken, DocumentState state)
    {
        _docstatus.Status = state;
        _docstatus.UpdatedAt = DateTime.Now;
        try
        {
            await _processingStatusRepository.UpdateAsync(_docstatus, CancellationToken.None);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<CreateDocumentResponse> AddDocument(IFormFile file, string userId, CancellationToken cancellationToken)
    {
        try
        {
            ValidationResult validation = _fileValidator.Validate(file);
            if (!validation.IsValid)
            {
                _logger.LogInformation($"{file} : {validation.Errors.ErrorsToString()}");
                throw new ServiceException(nameof(AddDocument), file, validation.Errors.ErrorsToString());
            }


            _document = new Document(file.ContentType, file.FileName, file.Name, file.Length, DateTime.Now, userId);
            _docstatus = new ProcessingStatus(_document.Id, DateTime.Now);

            await _processingStatusRepository.AddAsync(_docstatus, cancellationToken);


            var data = new ProcessedData(_document.Id, await GetFormattedText(await RecognizeText(file, cancellationToken), cancellationToken), DateTime.Now);
            await _processedDataRepository.AddAsync(data, cancellationToken);

            // Десериализация data.StructuredData в ApplicantProfile через System.Text.Json
            ApplicantProfile applicantProfile;
            if (string.IsNullOrWhiteSpace(data.StructuredData))
            {
                _logger.LogWarning("StructuredData is empty or null, creating empty ApplicantProfile");
                applicantProfile = new ApplicantProfile();
            }
            else
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true
                    };
                    string serializedText = data.StructuredData;
                    if (data.StructuredData.Contains("```json"))
                    {
                        serializedText = data.StructuredData.Replace("```json", "").Replace("```", "").Trim();
                    }

                    _logger.LogInformation($"Serialized text: {serializedText}");
                    applicantProfile = JsonSerializer.Deserialize<ApplicantProfile>(serializedText, options) ?? new ApplicantProfile();
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Failed to deserialize StructuredData into ApplicantProfile. Returning empty ApplicantProfile.");
                    applicantProfile = new ApplicantProfile();
                }
            }

            var response = new CreateDocumentResponse()
            {
                UserId = userId,
                DocumentId = _document.Id.ToString(),
                ApplicantProfile = applicantProfile
            };

            _logger.LogInformation($"Processed data for document {data.StructuredData}");

            await InvokeOnUpdateAsync(cancellationToken, DocumentState.Completed);
            _document.UploadedUntil = DateTime.Now;
            await AddFileToDataBase(_document, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            await InvokeOnUpdateAsync(cancellationToken, DocumentState.Error);
            throw;
        }
    }
}
