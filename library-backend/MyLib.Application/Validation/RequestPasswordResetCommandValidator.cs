using FluentValidation;
using MyLib.Application.Handlers.Users.Commands.RequestPasswordReset;

namespace MyLib.Application.Validation;

public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
} 