using FluentValidation;
using MyLib.Application.Handlers.Users.Commands.LoginUser;

namespace MyLib.Application.Validation;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Username or Email is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
} 