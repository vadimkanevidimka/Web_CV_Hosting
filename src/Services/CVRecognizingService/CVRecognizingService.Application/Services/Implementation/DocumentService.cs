using CVRecognizingService.Application.FluentValidation;
using CVRecognizingService.Application.Helpers.AICompanion;
using CVRecognizingService.Application.Helpers.PDFConverter;
using CVRecognizingService.Application.Helpers.PDFRecognizing;
using CVRecognizingService.Application.Services.Interfaces;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.ValueObjects.VODerivatives;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using DotnetGeminiSDK.Client.Interfaces;
using Events_Web_application.Application.Services.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.Services.Implementation
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger _logger;
        private readonly DocumentRepository _repository;
        private readonly FileValidator _fileValidator;
        private readonly GeminiAITextChat _chat;
        public DocumentService(IGeminiClient geminiClient, ILogger<DocumentService> logger, IRepository<BaseDocument> repository, IValidator<IFormFile> fileValidator)
        {
            _logger = logger;
            _chat = new GeminiAITextChat(geminiClient);
            _repository = (DocumentRepository)repository;
            _fileValidator = (FileValidator)fileValidator;
        }

        private async Task<long> AddFileToDataBase(BaseDocument document, CancellationToken cancellationToken)
        {
            var result = await _repository.Add(document, cancellationToken);

            _logger.LogInformation($"Document from {document.FileName} was recognized and added to database");

            return result;
        }

        private async Task<string> GetFormattedText(string nonFormatedText, CancellationToken cancellationToken)
        {
            var result = await _chat.GetAnswer(nonFormatedText, cancellationToken);

            _logger.LogInformation($"Text|\n {nonFormatedText} \n formatted {result.Candidates[0].Content.Parts[0].Text}");

            return result.Candidates[0].Content.Parts[0].Text;
        }

        private async Task<string> RecognizeText(IFormFile file, CancellationToken cancellationToken)
        {
            var recognizedText = new PDFRecognizer(await file.GetBytesAsync(cancellationToken));

            _logger.LogInformation($"Text from {file} recognized {recognizedText.RecognizedText}");

            return recognizedText.RecognizedText;
        }

        public async Task<BaseDocument> AddDocument(IFormFile file, CancellationToken cancellationToken)
        {
            var doc = new BaseDocument
                    (
                        file.ContentType,
                        file.FileName,
                        file.Name,
                        file.Length,
                        DateTime.Now,
                        new User("Vadim", "vadimdd5@gmail.com")
                    );

            ValidationResult validation = _fileValidator.Validate(file);
            if (!validation.IsValid)
            {
                _logger.LogInformation($"{file} : {validation.Errors}");
                throw new ServiceException(nameof(AddDocument), file, validation.Errors.ToString())
            }

            doc.ProcessingStatus = new ProcessingStatus(doc.Id, "Pending", DateTime.Now);
            doc.ProcessedData = new ProcessedData(doc.Id, await GetFormattedText(await RecognizeText(file, cancellationToken), cancellationToken), DateTime.Now);
            doc.UploadedUntil = DateTime.Now;
            doc.ProcessingStatus.Status = "Finished";
            doc.ProcessingStatus.UpdatedAt = DateTime.Now;

            await AddFileToDataBase(doc, cancellationToken);

            return doc;
        }

        public async Task<IEnumerable<BaseDocument>> GetDocuments(CancellationToken cancellationToken)
        {
            return await _repository.GetAll(cancellationToken);
        }
    }
}
