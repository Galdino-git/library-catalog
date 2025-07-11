using FluentValidation.TestHelper;
using MyLib.Application.Handlers.Users.Commands.LoginUser;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator;

public class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator;

    public LoginUserCommandValidatorTests()
    {
        _validator = new LoginUserCommandValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid_Command()
    {
        var command = new LoginUserCommand
        {
            UsernameOrEmail = "testuser",
            Password = "TestPass123!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_UsernameOrEmail_Is_Empty()
    {
        var command = new LoginUserCommand
        {
            UsernameOrEmail = "",
            Password = "TestPass123!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UsernameOrEmail);
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Empty()
    {
        var command = new LoginUserCommand
        {
            UsernameOrEmail = "testuser",
            Password = ""
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}