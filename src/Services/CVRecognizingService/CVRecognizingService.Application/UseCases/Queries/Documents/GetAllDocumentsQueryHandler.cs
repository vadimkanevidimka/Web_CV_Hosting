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
    private readonly IRepository<Document> _documentRepository;
    private readonly IMapper _mapper;

    public GetAllDocumentsQueryHandler(

        ILogger<GetAllDocumentsQueryHandler> logger,
        IRepository<Document> documentRepository,
        IMapper mapper)
    {
        _logger = logger;
        _documentRepository = (DocumentRepository) documentRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<BaseDocumentDto>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<Document>, IEnumerable<BaseDocumentDto>>(await _documentRepository.GetAllAsync(cancellationToken));
    }
}
