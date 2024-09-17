using CVRecognizingService.Domain.Entities;
using MediatR;

namespace CVRecognizingService.Application.Queries.Documents;

public class GetDocumentByIdQuery 
    : IRequest<BaseDocument>
{
    public string Id { get; set; }
}
