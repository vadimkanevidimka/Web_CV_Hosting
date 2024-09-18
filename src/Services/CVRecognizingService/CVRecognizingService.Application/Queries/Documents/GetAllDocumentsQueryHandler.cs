using AutoMapper;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using CVRecognizingService.Domain.DTOs.Outgoing;
using CVRecognizingService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.Queries.Documents;

internal class GetAllDocumentsQueryHandler 
    : IRequestHandler<GetAllDocumentsQuery, IEnumerable<BaseDocumentDto>>
{
    private readonly ILogger<GetAllDocumentsQueryHandler> _logger;
    private readonly DocumentService _documentService;
    private readonly IMapper _mapper;

    public GetAllDocumentsQueryHandler(
        ILogger<GetAllDocumentsQueryHandler> logger,
        IService documentService,
        IMapper mapper)
    {
        _logger = logger;
        _documentService = (DocumentService)documentService;
        _mapper = mapper;
    }
    public async Task<IEnumerable<BaseDocumentDto>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<BaseDocument>, IEnumerable<BaseDocumentDto>>(await _documentService.GetDocuments(cancellationToken));
    }
}
