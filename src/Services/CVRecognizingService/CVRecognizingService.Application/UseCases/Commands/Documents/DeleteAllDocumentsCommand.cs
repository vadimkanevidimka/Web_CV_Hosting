using MediatR;

namespace CVRecognizingService.Application.UseCases.Commands.Documents
{
    public class DeleteAllDocumentsCommand : IRequest<long> { }
}
