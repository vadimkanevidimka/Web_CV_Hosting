using AutoMapper;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.DTOs.Outgoing;
using CVRecognizingService.Domain.Entities;
using Events_Web_application.Application.Services.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace CVRecognizingService.Application.UseCases.Queries.Documents;

public class GetDocumentByIdQueryHandler
    : IRequestHandler<GetDocumentByIdQuery, BaseDocumentDto>
{
    private readonly ILogger<GetDocumentByIdQueryHandler> _logger;
    private readonly IRepository<BaseDocument> _documentRepository;
    private readonly IMapper _mapper;

    public GetDocumentByIdQueryHandler(
        IRepository<BaseDocument> documentRepository,
        ILogger<GetDocumentByIdQueryHandler> logger,
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _documentRepository = documentRepository;
    }
    public async Task<BaseDocumentDto> Handle(
        GetDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Get by id request: {request.Id}");
        return _mapper.Map<BaseDocument, BaseDocumentDto>(await GetById(ObjectId.Parse(request.Id), cancellationToken));
    }

    public async Task<BaseDocument> GetById(ObjectId id, CancellationToken cancellationToken)
    {
        if (id == ObjectId.Empty) throw new ServiceException(nameof(GetById), id, "Id is not correct or not found");
        return await _documentRepository.Get(id, cancellationToken);
    }
}
