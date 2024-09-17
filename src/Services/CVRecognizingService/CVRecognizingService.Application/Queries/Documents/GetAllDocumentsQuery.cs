using CVRecognizingService.Domain.Entities;
using MediatR;

namespace CVRecognizingService.Application.Queries.Documents
{
    public class GetAllDocumentsQuery 
        : IRequest<IEnumerable<BaseDocument>> {
    }
}
