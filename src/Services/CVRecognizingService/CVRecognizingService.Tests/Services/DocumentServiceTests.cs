using AutoMapper;
using CVRecognizingService.Application.FluentValidation;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.UseCases.Commands.Document;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using DotnetGeminiSDK.Client.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.Linq;
using System.Net.Mime;
using System.Reflection.PortableExecutable;

namespace CVRecognizingService.Tests.Services
{
    public class CreateDocumentCommandTests
    {
        /// <summary>
        /// Test cases
        /// </summary>
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

        private readonly DocumentService _docService;
        private readonly Mock<DocumentRepository> _DOCRepositoryMock;
        private readonly Mock<ProcessedDataRepository> _PDRepositoryMock;
        private readonly Mock<ProcessingLogRepository> _PLRepositoryMock;
        private readonly Mock<ProcessingStatusRepository> _PSRepositoryMock;
        private readonly Mock<IGeminiClient> _clientMock;
        private readonly FileValidator _fileValidatorMock;
        private readonly Mock<ILogger<DocumentService>> _logger;
        private readonly Mock<FormFile> _mockFile;
        private readonly Mock<IMongoClient> _mongoClientMock;
        private readonly CreateDocumentCommandHandler _handler;

        public CreateDocumentCommandTests()
        {
            _clientMock = new Mock<IGeminiClient>();
            _mongoClientMock = new Mock<IMongoClient>();
            _DOCRepositoryMock = new Mock<DocumentRepository>(_mongoClientMock.Object);
            _PDRepositoryMock = new Mock<ProcessedDataRepository>(_mongoClientMock.Object);
            _PLRepositoryMock = new Mock<ProcessingLogRepository>(_mongoClientMock.Object);
            _PSRepositoryMock = new Mock<ProcessingStatusRepository>(_mongoClientMock.Object);
            _fileValidatorMock = new FileValidator();
            _logger = new Mock<ILogger<DocumentService>>();
            _docService = new DocumentService(_clientMock.Object, _logger.Object, _DOCRepositoryMock.Object, _PSRepositoryMock.Object, _PDRepositoryMock.Object, _PLRepositoryMock.Object, _fileValidatorMock);
            _mockFile = new Mock<FormFile>();
        }

        [Fact]
        public async Task AddDocument_Should_ReturnError_WhenFileIsWrongFormat()
        {
            try
            {
                var result = await _docService.AddDocument(WrongFileFormat, default);
                Assert.Null(result);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("Wrong file type.", StringComparison.OrdinalIgnoreCase));
            }
        }
        [Fact]
        public async Task AddDocument_Should_ReturnError_WrongFileSizeZero()
        {
            try
            {
                var result = await _docService.AddDocument(WrongFileLengthZero, default);
                Assert.Null(result);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("File is too big (More than 5MB) or too small", StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public async Task AddDocument_Should_ReturnError_WrongFileSize()
        {
            try
            {
                var result = await _docService.AddDocument(WrongFileLengthMoreThanFiveMB, default);
                Assert.Null(result);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("File is too big (More than 5MB) or too small", StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public async Task AddDocument_Should_ReturnError_WrongFileName()
        {
            try
            {
                var result = await _docService.AddDocument(WrongFileName, default);
                Assert.Null(result);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("File name is too long.", StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}