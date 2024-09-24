using AuthService.Buisness.Dtos.User;
using FluentValidation;

namespace AuthService.Buisness.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Please, enter the username");

        RuleFor(x => x.Password)
            .Password();

        RuleFor(x => x.ConfirmPassword)
            .Password()
            .Equal(x => x.Password).WithMessage("The passwords you entered do not match");
    }
}