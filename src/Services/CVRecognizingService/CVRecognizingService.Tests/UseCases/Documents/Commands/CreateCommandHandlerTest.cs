using AutoMapper;
using CVRecognizingService.Application.FluentValidation;
using CVRecognizingService.Application.Helpers.AiPdfComanion;
using CVRecognizingService.Application.UseCases.Commands.Documents;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using DotnetGeminiSDK.Client.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;

namespace CVRecognizingService.Tests.UseCases.Documents.Commands
{
    public class CreateCommandHandlerTest
    {
        /// <summary>
        /// Test cases
        /// </summary>
        /// 
        private static FormFile CorrectFile = new FormFile(Stream.Null, 1024, 5000, "wrongFile", "wrong!")
        {
            Headers = new HeaderDictionary(
               new Dictionary<string, StringValues>()
           {
                    { "ContentType","application/pdf" },
           })
        };

        private static FormFile WrongFileFormat = new FormFile(Stream.Null, 1024, 5000, "wrongFile", "wrong!")
        {
            Headers = new HeaderDictionary(
                new Dictionary<string, StringValues>()
            {
                    { "ContentType","application/json" },
            })
        };

        private static FormFile WrongFileLengthZero = new FormFile(Stream.Null, 1024, 0, "", "")
        {
            Headers = new HeaderDictionary(
                new Dictionary<string, StringValues>()
            {
                    { "ContentType","application/pdf" },
            })
        };

        private static FormFile WrongFileName = new FormFile(
            Stream.Null,
            1024,
            5000,
            "",
            ""
            )
        {
            Headers = new HeaderDictionary(
                new Dictionary<string, StringValues>()
            {
                    { "ContentType","application/pdf" },
            })
        };

        private static FormFile WrongFileLengthMoreThanFiveMB = new FormFile(Stream.Null, 1024, 5242881, "wrongFile", "wrong!")
        {
            Headers = new HeaderDictionary(
                new Dictionary<string, StringValues>()
            {
                    { "ContentType","application/pdf" },
            })
        };





        /// <summary>
        /// Start testing
        /// </summary>
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateDocumentCommandHandler _handler;
        private readonly Mock<DocumentRepository> _documentRepository;
        private readonly Mock<ProcessingStatusRepository> _processingStatusRepository;
        private readonly Mock<ProcessedDataRepository> _processedDataRepository;
        private readonly Mock<ProcessingLogRepository> _processingLogRepository;
        private readonly Mock<ILogger<CreateDocumentCommandHandler>> _logger;
        private readonly Mock<IGeminiClient> _clientMock;
        private readonly DbContext _dbContext;

        private readonly GeminiAITextChat _chat;
        private readonly Mock<FileValidator> _fileValidatorMock;

        public CreateCommandHandlerTest()
        {
            IOptions<ConnectionSettings> options = Options.Create(new ConnectionSettings() {ConnectionString="", Database = ""});
            _dbContext = new DbContext(options);
            _mapperMock = new Mock<IMapper>();
            _clientMock = new Mock<IGeminiClient>();
            _fileValidatorMock = new Mock<FileValidator>();
            _logger = new Mock<ILogger<CreateDocumentCommandHandler>>();
            _documentRepository = new Mock<DocumentRepository>(_dbContext);
            _processedDataRepository = new Mock<ProcessedDataRepository>(_dbContext);
            _processingLogRepository = new Mock<ProcessingLogRepository>(_dbContext);
            _processingStatusRepository = new Mock<ProcessingStatusRepository>(_dbContext);




            _handler = new CreateDocumentCommandHandler(
                _clientMock.Object,
                _fileValidatorMock.Object,
                _logger.Object,
                _documentRepository.Object,
                _processingStatusRepository.Object,
                _processedDataRepository.Object,
                _processingLogRepository.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenCreateDocumentWrongFormat()
        {
            // Arrange
            CreateDocumentCommand createDocumentCommand = new CreateDocumentCommand(WrongFileFormat);

            var result = await _handler.Handle(createDocumentCommand, default);

            Assert.Contains("Failed", result);
        }

        public async Task Handle_ShouldReturnError_WhenCreateDocumentWrongFileLengthZero()
        {
            // Arrange
            CreateDocumentCommand createDocumentCommand = new CreateDocumentCommand(WrongFileLengthZero);

            var result = await _handler.Handle(createDocumentCommand, default);

            Assert.Contains("Failed", result);
        }

        public async Task Handle_ShouldReturnError_WhenCreateDocumentWrongFileName()
        {
            // Arrange
            CreateDocumentCommand createDocumentCommand = new CreateDocumentCommand(WrongFileName);

            var result = await _handler.Handle(createDocumentCommand, default);

            Assert.Contains("Failed", result);
        }

        public async Task Handle_ShouldReturnError_WhenCreateDocumentWrongFileLengthMoreThanFiveMB()
        {
            // Arrange
            CreateDocumentCommand createDocumentCommand = new CreateDocumentCommand(WrongFileLengthMoreThanFiveMB);

            var result = await _handler.Handle(createDocumentCommand, default);

            Assert.Contains("Failed", result);
        }
    }
}
