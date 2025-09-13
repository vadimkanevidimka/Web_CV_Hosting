using FluentValidation;
using ProfileService.Application.UseCases.Commands;

namespace ProfileService.Application.Validators;

public class CreateEducationCommandValidator : AbstractValidator<CreateEducationCommand>
{
    public CreateEducationCommandValidator()
    {
        RuleFor(x => x.InstitutionName)
            .NotEmpty().WithMessage("Название учебного заведения обязательно.");

        RuleFor(x => x.Degree)
            .NotEmpty().WithMessage("Степень или диплом обязательны.");

        RuleFor(x => x.EndYear)
            .InclusiveBetween(1900, DateTime.Now.Year + 5).WithMessage("Год окончания должен быть в разумных пределах.");
    }
}

public class CreateSalaryExpectationsCommandValidator : AbstractValidator<CreateSalaryExpectationsCommand>
{
    public CreateSalaryExpectationsCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Сумма должна быть больше нуля.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Валюта обязательна.");
    }
}