using AutoMapper;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.DTOs.Outgoing;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.UseCases.Queries.Documents;

internal class GetAllDocumentsQueryHandler
    : IRequestHandler<GetAllDocumentsQuery, IEnumerable<BaseDocumentDto>>
{
    private readonly ILogger<GetAllDocumentsQueryHandler> _logger;
    private readonly DocumentRepository _documentRepository;
    private readonly IMapper _mapper;

    public GetAllDocumentsQueryHandler(
        ILogger<GetAllDocumentsQueryHandler> logger,
        IRepository<Document> documentRepository,
        IMapper mapper)
    {
        _logger = logger;
        _documentRepository = (DocumentRepository)documentRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<BaseDocumentDto>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        var a = await _documentRepository.GetAllAsync(cancellationToken);
        _logger.Log(LogLevel.Information, $"Recieved {a?.Count()} records");
        return _mapper.Map<IEnumerable<Document>, IEnumerable<BaseDocumentDto>>(a);
    }
}
