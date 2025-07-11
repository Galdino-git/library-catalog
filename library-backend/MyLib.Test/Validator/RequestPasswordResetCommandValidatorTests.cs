using FluentValidation.TestHelper;
using MyLib.Application.Handlers.Users.Commands.RequestPasswordReset;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator;

public class RequestPasswordResetCommandValidatorTests
{
    private readonly RequestPasswordResetCommandValidator _validator;

    public RequestPasswordResetCommandValidatorTests()
    {
        _validator = new RequestPasswordResetCommandValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid_Command()
    {
        var command = new RequestPasswordResetCommand
        {
            Email = "test@example.com"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Empty()
    {
        var command = new RequestPasswordResetCommand
        {
            Email = ""
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var command = new RequestPasswordResetCommand
        {
            Email = "invalid-email"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}