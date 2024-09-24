using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace CVRecognizingService.Application.FluentValidation
{
    public class FileValidator 
        : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x)
                .NotNull();

            RuleFor(x => x.Headers)
                .NotNull()
                .NotEmpty()
                .WithMessage("Wrong file type."); ;

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(4, 256)
                .WithMessage("File name is too long.");

            RuleFor(x => x.ContentType)
                .Equal("application/pdf")
                .WithMessage("Wrong file type.");

            RuleFor(x => x.Length)
                .InclusiveBetween(1, 5242880)
                .WithMessage("File is too big (More than 5MB) or too small");

            RuleFor(x => x.FileName)
                .NotEmpty()
                .Length(0, 256)
                .WithMessage("File name is too long.");
        }
    }

    public static class ValidatorErrorsExtension
    {
        public static string ErrorsToString(this List<ValidationFailure> validationFailures)
        {
            StringBuilder errors = new();
            foreach (var failure in validationFailures){
                 errors.Append(failure.ToString()+"\n");
            }
            return errors.ToString();
        }
    }
}
