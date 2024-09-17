using MediatR;

namespace CVRecognizingService.Application.Queries.Root;

public class GetRootQuery 
    : IRequest<Domain.DTOs.Incoming.Root>
{
    public string DocId { get; set; }
}
