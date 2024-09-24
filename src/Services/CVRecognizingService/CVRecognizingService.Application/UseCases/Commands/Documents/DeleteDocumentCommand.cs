using MediatR;

namespace CVRecognizingService.Application.UseCases.Commands.Documents
{
    public class DeleteDocumentCommand
        : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
