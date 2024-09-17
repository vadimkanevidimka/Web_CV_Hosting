using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace CVRecognizingService.Application.Commands.Document
{
    internal class DeleteDocumentCommandHandler 
        : IRequestHandler<DeleteDocumentCommand, bool>
    {

        private readonly ILogger<DeleteDocumentCommandHandler> _logger;
        private readonly DocumentService _documentService;

        public DeleteDocumentCommandHandler(
            ILogger<DeleteDocumentCommandHandler> logger,
            IService documentService)
        {
            _logger = logger;
            _documentService = (DocumentService)documentService;
        }
        public async Task<bool> Handle(
            DeleteDocumentCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _documentService.Delete(ObjectId.Parse(command.Id), cancellationToken);
            return result != 0;
        }
    }
}
