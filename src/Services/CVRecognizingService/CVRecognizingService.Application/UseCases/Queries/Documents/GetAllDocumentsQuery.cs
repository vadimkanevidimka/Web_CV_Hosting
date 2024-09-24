using CVRecognizingService.Domain.DTOs.Outgoing;
using MediatR;

namespace CVRecognizingService.Application.UseCases.Queries.Documents
{
    public class GetAllDocumentsQuery
        : IRequest<IEnumerable<BaseDocumentDto>>
    {
    }
}
