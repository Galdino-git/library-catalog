using FluentValidation.TestHelper;
using MyLib.Application.Handlers.Users.Commands.ResetPassword;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator;

public class ResetPasswordCommandValidatorTests
{
    private readonly ResetPasswordCommandValidator _validator;

    public ResetPasswordCommandValidatorTests()
    {
        _validator = new ResetPasswordCommandValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid_Command()
    {
        var command = new ResetPasswordCommand
        {
            Token = "valid-token",
            NewPassword = "NewTestPass123!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Token_Is_Empty()
    {
        var command = new ResetPasswordCommand
        {
            Token = "",
            NewPassword = "NewTestPass123!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Token);
    }

    [Fact]
    public void Should_Fail_When_NewPassword_Is_Too_Short()
    {
        var command = new ResetPasswordCommand
        {
            Token = "valid-token",
            NewPassword = "short"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Fail_When_NewPassword_Does_Not_Meet_Complexity_Requirements()
    {
        var command = new ResetPasswordCommand
        {
            Token = "valid-token",
            NewPassword = "simplepassword"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }
}