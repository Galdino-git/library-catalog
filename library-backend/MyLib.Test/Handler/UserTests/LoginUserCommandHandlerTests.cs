using MyLib.Application.Handlers.Users.Commands.LoginUser;
using MyLib.Application.Services;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.UserTests;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _handler = new LoginUserCommandHandler(_unitOfWorkMock.Object, _passwordServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Token_When_Credentials_Are_Valid()
    {
        var command = new LoginUserCommand { UsernameOrEmail = "testuser", Password = "TestPass123!" };
        var user = new User { Id = Guid.NewGuid(), Username = "testuser", Email = "test@example.com", PasswordHash = "hash" };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByEmailAsync(command.UsernameOrEmail)).ReturnsAsync((User?)null);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByUsernameAsync(command.UsernameOrEmail)).ReturnsAsync(user);
        _passwordServiceMock.Setup(x => x.VerifyPassword(command.Password, user.PasswordHash)).Returns(true);
        _jwtServiceMock.Setup(x => x.GenerateToken(user.Id.ToString(), user.Username, user.Email)).Returns("token");

        var result = await _handler.Handle(command, default);

        Assert.Equal("token", result);

        _unitOfWorkMock.Verify(x => x.UserRepository.GetByEmailAsync(command.UsernameOrEmail), Times.Once);
        _unitOfWorkMock.Verify(x => x.UserRepository.GetByUsernameAsync(command.UsernameOrEmail), Times.Once);
        _passwordServiceMock.Verify(x => x.VerifyPassword(command.Password, user.PasswordHash), Times.Once);
        _jwtServiceMock.Verify(x => x.GenerateToken(user.Id.ToString(), user.Username, user.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Not_Found()
    {
        var command = new LoginUserCommand { UsernameOrEmail = "notfound", Password = "TestPass123!" };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByEmailAsync(command.UsernameOrEmail)).ReturnsAsync((User?)null);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByUsernameAsync(command.UsernameOrEmail)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, default));

        _unitOfWorkMock.Verify(x => x.UserRepository.GetByEmailAsync(command.UsernameOrEmail), Times.Once);
        _unitOfWorkMock.Verify(x => x.UserRepository.GetByUsernameAsync(command.UsernameOrEmail), Times.Once);
        _passwordServiceMock.Verify(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _jwtServiceMock.Verify(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Password_Is_Invalid()
    {
        var command = new LoginUserCommand { UsernameOrEmail = "testuser", Password = "wrongpass" };
        var user = new User { Id = Guid.NewGuid(), Username = "testuser", Email = "test@example.com", PasswordHash = "hash" };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByEmailAsync(command.UsernameOrEmail)).ReturnsAsync((User?)null);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByUsernameAsync(command.UsernameOrEmail)).ReturnsAsync(user);
        _passwordServiceMock.Setup(x => x.VerifyPassword(command.Password, user.PasswordHash)).Returns(false);

        await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, default));

        _unitOfWorkMock.Verify(x => x.UserRepository.GetByEmailAsync(command.UsernameOrEmail), Times.Once);
        _unitOfWorkMock.Verify(x => x.UserRepository.GetByUsernameAsync(command.UsernameOrEmail), Times.Once);
        _passwordServiceMock.Verify(x => x.VerifyPassword(command.Password, user.PasswordHash), Times.Once);
        _jwtServiceMock.Verify(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}