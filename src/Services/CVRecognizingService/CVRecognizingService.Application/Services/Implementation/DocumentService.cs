using MongoDB.Bson;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DotnetGeminiSDK.Client.Interfaces;
using CVRecognizingService.Application.FluentValidation;
using CVRecognizingService.Application.Helpers.AiPdfComanion;
using CVRecognizingService.Application.Helpers.PDFConverter;
using CVRecognizingService.Application.Helpers.PDFRecognizing;
using CVRecognizingService.Application.Services.Interfaces;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.Enums;
using CVRecognizingService.Domain.Exeptions;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using Events_Web_application.Application.Services.Exceptions;

namespace CVRecognizingService.Application.Services.Implementation
{
    public class DocumentService : IService
    {
        /// <summary>
        /// Repositoies
        /// </summary>
        private readonly DocumentRepository _documentRepository;
        private readonly ProcessingStatusRepository _processingStatusRepository;
        private readonly ProcessedDataRepository _processedDataRepository;
        private readonly ProcessingLogRepository _processingLogRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger _logger;
        private readonly FileValidator _fileValidator;
        private readonly GeminiAITextChat _chat;

        /// <summary>
        /// 
        /// </summary>
        private event Update OnUpdate;
        private delegate void Update(CancellationToken cancellationToken, DocumentState state);
        private BaseDocument _document;
        private ProcessingStatus _docstatus;
        public DocumentService(
            IGeminiClient geminiClient, 
            ILogger<DocumentService> logger,
            IRepository<BaseDocument> documentRepository, 
            IRepository<ProcessingStatus> processingStatusRepository,
            IRepository<ProcessedData> processedDataRepository,
            IRepository<ProcessingLog> processedLogRepository,
            IValidator<IFormFile> fileValidator)
        {
            _logger = logger;
            _chat = new GeminiAITextChat(geminiClient);
            _documentRepository = (DocumentRepository)documentRepository;
            _processingStatusRepository = (ProcessingStatusRepository)processingStatusRepository;
            _processedDataRepository = (ProcessedDataRepository)processedDataRepository;
            _processingLogRepository = (ProcessingLogRepository)processedLogRepository;
            _fileValidator = (FileValidator)fileValidator;
            OnUpdate += UpdateDocumentState;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullObjectException"></exception>
        private async Task<long> AddFileToDataBase(BaseDocument document, CancellationToken cancellationToken)
        {
            if (document == null) throw new NullObjectException(nameof(document));

            var result = await _documentRepository.Add(document, cancellationToken);

            _logger.LogInformation($"Document from {document.FileName} was recognized and added to database");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nonFormatedText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<string> GetFormattedText(string nonFormatedText, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(nonFormatedText)) throw new ArgumentException(nameof(nonFormatedText));

            var result = await _chat.GetFormatedText(nonFormatedText, cancellationToken);

            OnUpdate.Invoke(cancellationToken, DocumentState.Processing);

            _logger.LogInformation($"Text|\n {nonFormatedText} \n formatted {result.Candidates[0].Content.Parts[0].Text}");

            return result.Candidates[0].Content.Parts[0].Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<BaseDocument> AddDocument(IFormFile file, CancellationToken cancellationToken)
        {
            _document = new BaseDocument
                    (
                        file.ContentType,
                        file.FileName,
                        file.Name,
                        file.Length,
                        DateTime.Now,
                       //new User("Vadim", "vadimdd5@gmail.com", "Admin")
                       new User()
                    );
            _docstatus = new ProcessingStatus(_document.Id, DateTime.Now);

            try
            {
                ValidationResult validation = _fileValidator.Validate(file);
                if (!validation.IsValid)
                {
                    _logger.LogInformation($"{file} : {validation.Errors}");
                    throw new ServiceException(nameof(AddDocument), file, validation.Errors.ToString());
                }
                await _processingStatusRepository.Add(_docstatus, cancellationToken);


                await _processedDataRepository.Add(new ProcessedData(_document.Id,await GetFormattedText(await RecognizeText(file, cancellationToken), cancellationToken), DateTime.Now), cancellationToken);


                OnUpdate(cancellationToken, DocumentState.Completed);
                _document.UploadedUntil = DateTime.Now;
                await AddFileToDataBase(_document, cancellationToken);
                return _document;
            }
            catch (Exception ex) 
            {
                OnUpdate(cancellationToken, DocumentState.Error);
                return null;
            }

        }
        /// <summary>
        /// Return document by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<BaseDocument> GetById(ObjectId id, CancellationToken cancellationToken){
            if (id == ObjectId.Empty) throw new ServiceException(nameof(GetById), id, "Id is not correct or not found");
            return await _documentRepository.Get(id, cancellationToken);
        }

        public async Task<long> Delete(ObjectId id, CancellationToken cancellationToken)
        {
            if (id == ObjectId.Empty) throw new ServiceException(nameof(GetById), id, "Id is not correct or not found");
            return await _documentRepository.Delete(id, cancellationToken);
        }

        public async Task<IEnumerable<BaseDocument>> GetDocuments(CancellationToken cancellationToken){
            return await _documentRepository.GetAll(cancellationToken);
        }

        private async void UpdateDocumentState(CancellationToken cancellationToken, DocumentState state){
            _docstatus.Status = state;
            _docstatus.UpdatedAt = DateTime.Now;
            await _processingStatusRepository.Update(_docstatus, cancellationToken);
        }
    }
}
