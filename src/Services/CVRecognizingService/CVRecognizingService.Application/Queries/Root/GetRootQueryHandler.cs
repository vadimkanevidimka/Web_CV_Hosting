using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
namespace CVRecognizingService.Application.Queries.Root;

internal class GetRootQueryHandler 
    : IRequestHandler<GetRootQuery, Domain.DTOs.Incoming.Root>
{
    private readonly ILogger<GetRootQueryHandler> _logger;
    private readonly DocumentService _documentService;

    public GetRootQueryHandler(
        ILogger<GetRootQueryHandler> logger,
        IService documentService)
    {
        _logger = logger;
        _documentService = (DocumentService)documentService;
    }
    public async Task<Domain.DTOs.Incoming.Root> Handle(GetRootQuery query, CancellationToken cancellationToken)
    {
        return await _documentService.GetDataFromDocument(ObjectId.Parse(query.DocId), cancellationToken);
    }
}
