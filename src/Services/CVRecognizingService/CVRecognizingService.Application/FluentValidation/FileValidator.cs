using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CVRecognizingService.Application.FluentValidation;

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        RuleFor(x => x)
            .NotNull();

        RuleFor(x => x.Headers)
            .NotNull()
            .NotEmpty().WithMessage("Wrong file type."); ;

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(0, 256).WithMessage("File name is too long.");

        RuleFor(x => x.ContentType)
            .Equal("application/pdf").WithMessage("Wrong file type.");

        RuleFor(x => x.Length)
            .InclusiveBetween(0, 5242880).WithMessage("File is too big. (More than 5MB)");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .Length(0, 256).WithMessage("File name is too long.");
    }
}
