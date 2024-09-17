using MediatR;

namespace CVRecognizingService.Application.Commands.Document
{
    public class DeleteDocumentCommand 
        : IRequest<bool> 
    {
        public string Id { get; set; }
    }
}
