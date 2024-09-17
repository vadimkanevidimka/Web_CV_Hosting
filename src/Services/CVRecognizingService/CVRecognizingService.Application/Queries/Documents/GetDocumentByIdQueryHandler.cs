using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using CVRecognizingService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace CVRecognizingService.Application.Queries.Documents;

public class GetDocumentByIdQueryHandler
    : IRequestHandler<GetDocumentByIdQuery, BaseDocument>
{
    private readonly ILogger<GetDocumentByIdQueryHandler> _logger;
    private readonly DocumentService _documentService;

    public GetDocumentByIdQueryHandler(
        ILogger<GetDocumentByIdQueryHandler> logger,
        IService documentService)
    {
        _logger = logger;
        _documentService = (DocumentService)documentService;
    }
    public async Task<BaseDocument> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Get by id request: {request.Id}");
        return await _documentService.GetById(ObjectId.Parse(request.Id), cancellationToken);
    }
}
