using FluentValidation;
using ProfileService.Application.UseCases.Commands;

namespace ProfileService.Application.Validators;

public class CreateApplicantProfileCommandValidator : AbstractValidator<CreateApplicantProfileCommand>
{
    public CreateApplicantProfileCommandValidator()
    {
        // Правила для основной информации о соискателе
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно для заполнения.")
            .MaximumLength(50).WithMessage("Имя не может превышать 50 символов.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна для заполнения.")
            .MaximumLength(50).WithMessage("Фамилия не может превышать 50 символов.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен для заполнения.")
            .EmailAddress().WithMessage("Укажите корректный адрес электронной почты.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Номер телефона обязателен для заполнения.")
            .Matches(@"^\+?[0-9\s-]{7,15}$").WithMessage("Укажите корректный номер телефона.");

        // Правило для вложенных коллекций (например, для образования)
        // Ensure that the collection is not null and then run a validator for each item.
        RuleForEach(x => x.Educations)
            .SetValidator(new CreateEducationCommandValidator());

        // Правило для вложенных опциональных объектов
        When(x => x.SalaryExpectations != null, () =>
        {
            RuleFor(x => x.SalaryExpectations)
                .SetValidator(new CreateSalaryExpectationsCommandValidator());
        });
    }
}