using CVRecognizingService.Domain.DTOs.Outgoing;
using MediatR;

namespace CVRecognizingService.Application.UseCases.Queries.Documents
{
    public class GetUserDocumentsQuery
        : IRequest<IEnumerable<BaseDocumentDto>>
    {
        public Guid UserId { get; set; }
    }
}
