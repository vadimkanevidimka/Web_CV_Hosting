using AutoMapper;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using CVRecognizingService.Domain.DTOs.Outgoing;
using CVRecognizingService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace CVRecognizingService.Application.Queries.Documents;

public class GetDocumentByIdQueryHandler
    : IRequestHandler<GetDocumentByIdQuery, BaseDocumentDto>
{
    private readonly ILogger<GetDocumentByIdQueryHandler> _logger;
    private readonly DocumentService _documentService;
    private readonly IMapper _mapper;

    public GetDocumentByIdQueryHandler(
        ILogger<GetDocumentByIdQueryHandler> logger,
        IService documentService,
        IMapper mapper)
    {
        _logger = logger;
        _documentService = (DocumentService)documentService;
        _mapper = mapper;
    }
    public async Task<BaseDocumentDto> Handle(
        GetDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Get by id request: {request.Id}");
        return _mapper.Map<BaseDocument, BaseDocumentDto>(await _documentService.GetById(ObjectId.Parse(request.Id), cancellationToken));
    }
}
