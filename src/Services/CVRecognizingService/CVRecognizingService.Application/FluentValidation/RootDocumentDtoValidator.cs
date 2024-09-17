using CVRecognizingService.Domain.DTOs.Outgoing;
using FluentValidation;

namespace CVRecognizingService.Application.FluentValidation
{
    internal class DocumentDtoValidator : AbstractValidator<DocumentDto>
    {
        public DocumentDtoValidator() 
        {
            RuleFor(x=>x.Document)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ProcessedData)
                .NotNull()
                .NotEmpty();

            RuleFor(x=>x.ProcessingLog)
                .NotNull()
                .NotEmpty();

            RuleFor(x=>x.ProcessingStatus)
                .NotNull()
                .NotEmpty();
        }
    }
}
