using CVRecognizingService.Application.UseCases.Commands.Documents;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CVRecognizingService.Tests.UseCases.Documents.Commands
{
    public class DeleteCommandHandlerTests
    {
        
        private readonly DeleteDocumentCommandHandler _handler;
        private readonly Mock<DocumentRepository> _documentRepository;
        private readonly Mock<ILogger<DeleteDocumentCommandHandler>> _logger;
        private readonly DbContext _dbContext;
        public DeleteCommandHandlerTests()
        {
            IOptions<ConnectionSettings> options = Options.Create(new ConnectionSettings() { ConnectionString = "", Database = "" });
            _dbContext = new DbContext(options);
            _logger = new Mock<ILogger<DeleteDocumentCommandHandler>>();
            _documentRepository = new Mock<DocumentRepository>(_dbContext);

            _handler = new DeleteDocumentCommandHandler(
                _logger.Object,
                _documentRepository.Object
                );
        }


        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenDeletionIdIsEmprty()
        {
            // Arrange
            DeleteDocumentCommand deletecommand = new DeleteDocumentCommand();
            deletecommand.Id = string.Empty;

            var result = await _handler.Handle(deletecommand, default);

            Assert.False(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenDeletionIdIsNull()
        {
            // Arrange
            DeleteDocumentCommand deletecommand = new DeleteDocumentCommand();
            deletecommand.Id = null;

            var result = await _handler.Handle(deletecommand, default);

            Assert.False(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenDeletionIdIsWrong()
        {
            // Arrange
            DeleteDocumentCommand deletecommand = new DeleteDocumentCommand();
            deletecommand.Id = "HelloWorld!";

            var result = await _handler.Handle(deletecommand, default);

            Assert.False(result);
        }
    }
}
