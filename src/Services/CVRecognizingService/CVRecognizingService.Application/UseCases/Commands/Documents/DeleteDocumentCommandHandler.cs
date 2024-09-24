using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using Events_Web_application.Application.Services.Exceptions;
using MediatR;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.UseCases.Commands.Documents
{
    public class DeleteDocumentCommandHandler
        : IRequestHandler<DeleteDocumentCommand, bool>
    {

        private readonly ILogger<DeleteDocumentCommandHandler> _logger;
        private readonly IRepository<Document> _documentRepository;

        public DeleteDocumentCommandHandler(
            ILogger<DeleteDocumentCommandHandler> logger,
            IRepository<Document> documentRepository)
        {
            _logger = logger;
            _documentRepository = documentRepository;
        }
        public async Task<bool> Handle(
            DeleteDocumentCommand command,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Id)) throw new ServiceException(nameof(Handle), command.Id, "Id is not correct or not found");
            var result = await _documentRepository.DeleteAsync(ObjectId.Parse(command.Id), cancellationToken);
            return result != 0;
        }
    }
}
