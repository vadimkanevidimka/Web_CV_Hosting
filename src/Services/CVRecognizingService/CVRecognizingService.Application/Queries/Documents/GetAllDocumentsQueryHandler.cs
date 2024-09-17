using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using CVRecognizingService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.Queries.Documents;

internal class GetAllDocumentsQueryHandler 
    : IRequestHandler<GetAllDocumentsQuery, IEnumerable<BaseDocument>>
{
    private readonly ILogger<GetAllDocumentsQueryHandler> _logger;
    private readonly DocumentService _documentService;

    public GetAllDocumentsQueryHandler(
        ILogger<GetAllDocumentsQueryHandler> logger,
        IService documentService)
    {
        _logger = logger;
        _documentService = (DocumentService)documentService;
    }
    public async Task<IEnumerable<BaseDocument>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        return await _documentService.GetDocuments(cancellationToken);
    }
}
