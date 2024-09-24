using MediatR;
using Microsoft.Extensions.Logging;
using Events_Web_application.Application.Services.Exceptions;
using CVRecognizingService.Application.FluentValidation;
using CVRecognizingService.Application.Helpers.PDFRecognizing;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.Enums;
using CVRecognizingService.Domain.Exeptions;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using CVRecognizingService.Application.Helpers.AiPdfComanion;
using CVRecognizingService.Application.Helpers.PDFConverter;
using CVRecognizingService.Domain.Abstracts.Repo;
using DotnetGeminiSDK.Client.Interfaces;
using FluentValidation.Results;
using FluentValidation;

namespace CVRecognizingService.Application.UseCases.Commands.Documents;

public class CreateDocumentCommandHandler
    : IRequestHandler<CreateDocumentCommand, string>
{
    private readonly ILogger<CreateDocumentCommandHandler> _logger;

    /// <summary>
    /// Repositoies
    /// </summary>
    private readonly DocumentRepository _documentRepository;
    private readonly ProcessingStatusRepository _processingStatusRepository;
    private readonly ProcessedDataRepository _processedDataRepository;
    private readonly ProcessingLogRepository _processingLogRepository;

    private readonly GeminiAITextChat _chat;
    private readonly FileValidator _fileValidator;

    private event Update OnUpdate;
    private delegate void Update(CancellationToken cancellationToken, DocumentState state);
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

    public async Task<string> Handle(
        CreateDocumentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"{request.File.FileName} started recognizing");
            var result = await AddDocument(request.File, cancellationToken);
            return result.Id.ToString();
        }
        catch (ServiceException ex)
        {
            _logger.LogInformation($"{request.File.FileName} finished recognizing with error: {ex.Value}");
            return $"Failed service operation: {ex.Operation}, error: {ex.Message} with value: {ex.Value}";
        }
    }

    private async Task<long> AddFileToDataBase(Document document, CancellationToken cancellationToken)
    {
        if (document == null) throw new NullObjectException(nameof(document));

        var result = await _documentRepository.Add(document, cancellationToken);

        _logger.LogInformation($"Document from {document.FileName} was recognized and added to database");

        return result;
    }

    private async Task<string> GetFormattedText(string nonFormatedText, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(nonFormatedText)) throw new ArgumentException(nameof(nonFormatedText));

        var result = await _chat.GetFormatedText(nonFormatedText, cancellationToken);

        OnUpdate.Invoke(cancellationToken, DocumentState.Processing);

        _logger.LogInformation($"Text|\n {nonFormatedText} \n formatted {result.Candidates[0].Content.Parts[0].Text}");

        return result.Candidates[0].Content.Parts[0].Text;
    }

    private async Task<string> RecognizeText(IFormFile file, CancellationToken cancellationToken)
    {
        var recognizedText = new PDFRecognizer(await file.GetBytesAsync(cancellationToken));

        OnUpdate.Invoke(cancellationToken, DocumentState.Processing);

        _logger.LogInformation($"Text from {file} recognized {recognizedText.RecognizedText}");

        var isFileCV = await _chat.IsCV(recognizedText.RecognizedText, cancellationToken);
        if (isFileCV.Candidates[0].Content.Parts[0].Text.Contains("false", StringComparison.OrdinalIgnoreCase)
            || recognizedText.RecognizedText.Length == 0)
        {
            throw new ServiceException(nameof(this.RecognizeText), recognizedText.RecognizedText, "File is not Cover Leter or Resume");
        }
        return recognizedText.RecognizedText;
    }

    private async void UpdateDocumentState(CancellationToken cancellationToken, DocumentState state)
    {
        _docstatus.Status = state;
        _docstatus.UpdatedAt = DateTime.Now;
        await _processingStatusRepository.Update(_docstatus, cancellationToken);
    }

    public async Task<Document> AddDocument(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            ValidationResult validation = _fileValidator.Validate(file);
            if (!validation.IsValid)
            {
                _logger.LogInformation($"{file} : {validation.Errors.ErrorsToString()}");
                throw new ServiceException(nameof(AddDocument), file, validation.Errors.ErrorsToString());
            }


            _document = new Document(file.ContentType, file.FileName, file.Name, file.Length, DateTime.Now, new User());
            _docstatus = new ProcessingStatus(_document.Id, DateTime.Now);

            await _processingStatusRepository.Add(_docstatus, cancellationToken);


            await _processedDataRepository.Add(new ProcessedData(_document.Id, await GetFormattedText(await RecognizeText(file, cancellationToken), cancellationToken), DateTime.Now), cancellationToken);


            OnUpdate(cancellationToken, DocumentState.Completed);
            _document.UploadedUntil = DateTime.Now;
            await AddFileToDataBase(_document, cancellationToken);
            return _document;
        }
        catch (Exception ex)
        {
            OnUpdate(cancellationToken, DocumentState.Error);
            throw;
        }
    }
}
