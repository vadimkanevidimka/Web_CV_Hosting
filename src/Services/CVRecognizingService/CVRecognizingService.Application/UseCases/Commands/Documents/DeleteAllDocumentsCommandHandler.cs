using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
namespace CVRecognizingService.Application.UseCases.Commands.Documents
{
    public class DeleteAllDocumentsCommandHandler : IRequestHandler<DeleteAllDocumentsCommand, long>
    {

        private readonly ILogger<DeleteAllDocumentsCommandHandler> _logger;
        private readonly DocumentRepository _documentRepository;

        public DeleteAllDocumentsCommandHandler(
            ILogger<DeleteAllDocumentsCommandHandler> logger,
            IRepository<Domain.Entities.Document> documentRepository)
        {
            _logger = logger;
            _documentRepository = (DocumentRepository)documentRepository;
        }
        public async Task<long> Handle(
            DeleteAllDocumentsCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _documentRepository.DeleteAllAsync(cancellationToken);
            _logger.Log(LogLevel.Information, $"Deleted {result} records");
            return result;
        }
    }
}