using MediatR;

namespace CVRecognizingService.Application.UseCases.Commands.Document
{
    public class DeleteDocumentCommand
        : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
