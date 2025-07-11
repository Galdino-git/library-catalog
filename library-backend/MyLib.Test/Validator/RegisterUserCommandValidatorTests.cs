using FluentValidation.TestHelper;
using MyLib.Application.Handlers.Users.Commands.RegisterUser;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorTests()
    {
        _validator = new RegisterUserCommandValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid_Command()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "TestPass123!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Username_Is_Empty()
    {
        var command = new RegisterUserCommand
        {
            Username = "",
            Email = "test@example.com",
            Password = "TestPass123!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "invalid-email",
            Password = "TestPass123!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Too_Short()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "short"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_Does_Not_Meet_Complexity_Requirements()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "simplepassword"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}