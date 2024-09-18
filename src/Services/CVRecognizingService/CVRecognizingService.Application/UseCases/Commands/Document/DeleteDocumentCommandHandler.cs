using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using Events_Web_application.Application.Services.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace CVRecognizingService.Application.UseCases.Commands.Document
{
    internal class DeleteDocumentCommandHandler
        : IRequestHandler<DeleteDocumentCommand, bool>
    {

        private readonly ILogger<DeleteDocumentCommandHandler> _logger;
        private readonly IRepository<BaseDocument> _documentRepository;

        public DeleteDocumentCommandHandler(
            ILogger<DeleteDocumentCommandHandler> logger,
            IRepository<BaseDocument> documentRepository)
        {
            _logger = logger;
            _documentRepository = documentRepository;
        }
        public async Task<bool> Handle(
            DeleteDocumentCommand command,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Id)) throw new ServiceException(nameof(Handle), command.Id, "Id is not correct or not found");
            var result = await _documentRepository.Delete(ObjectId.Parse(command.Id), cancellationToken);
            return result != 0;
        }
    }
}
