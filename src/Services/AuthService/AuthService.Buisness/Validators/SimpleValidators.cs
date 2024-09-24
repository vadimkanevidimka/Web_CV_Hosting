using FluentValidation;

namespace AuthService.Buisness.Validators;

public static class SimpleValidators
{
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Please, enter the password")
            .Length(4, 15).WithMessage("Password must be between 4 and 15 characters");
    }
}