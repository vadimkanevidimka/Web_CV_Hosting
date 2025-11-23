using AutoMapper;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.DTOs.Outgoing;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using Events_Web_application.Application.Services.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.UseCases.Queries.Documents
{
    public class GetUserDocumentsQueryHandler
        : IRequestHandler<GetUserDocumentsQuery, IEnumerable<BaseDocumentDto>>
    {
        private readonly ILogger<GetUserDocumentsQueryHandler> _logger;
        private readonly DocumentRepository _documentRepository;
        private readonly IMapper _mapper;

        public GetUserDocumentsQueryHandler(
            IRepository<Document> documentRepository,
            ILogger<GetUserDocumentsQueryHandler> logger,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _documentRepository = (DocumentRepository)documentRepository;
        }

        public async Task<IEnumerable<Document>> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty) throw new ServiceException(nameof(GetById), id, "Id is not correct or not found");
            return await _documentRepository.GetByUserIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<BaseDocumentDto>> Handle(
            GetUserDocumentsQuery request,
            CancellationToken cancellationToken)
        {
            var collection = await GetById(request.UserId, cancellationToken);
            _logger.LogInformation($"Get documents by user id request: {request.UserId}");
            return _mapper.Map<IEnumerable<Document>, IEnumerable<BaseDocumentDto>>(collection);
        }
    }
}
