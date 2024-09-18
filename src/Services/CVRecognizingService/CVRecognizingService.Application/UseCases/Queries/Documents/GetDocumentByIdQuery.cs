using CVRecognizingService.Domain.DTOs.Outgoing;
using MediatR;

namespace CVRecognizingService.Application.UseCases.Queries.Documents;

public class GetDocumentByIdQuery
    : IRequest<BaseDocumentDto>
{
    public string Id { get; set; }
}
