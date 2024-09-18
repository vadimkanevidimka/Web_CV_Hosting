using CVRecognizingService.Domain.DTOs.Outgoing;
using MediatR;

namespace CVRecognizingService.Application.Queries.Documents
{
    public class GetAllDocumentsQuery 
        : IRequest<IEnumerable<BaseDocumentDto>> {
    }
}
