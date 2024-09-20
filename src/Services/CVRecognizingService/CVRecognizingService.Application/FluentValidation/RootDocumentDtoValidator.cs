using FluentValidation;
using CVRecognizingService.Domain.DTOs.Outgoing;

namespace CVRecognizingService.Application.FluentValidation
{
    internal class RootDocumentDtoValidator : AbstractValidator<RootDocumentDto>
    {
        public RootDocumentDtoValidator() 
        {
            RuleFor(x=>x.Document).NotNull().NotEmpty();
            RuleFor(x=>x.ProcessedData).NotNull().NotEmpty();
            RuleFor(x=>x.ProcessingLog).NotNull().NotEmpty();
            RuleFor(x=>x.ProcessingStatus).NotNull().NotEmpty();
        }
    }
}
