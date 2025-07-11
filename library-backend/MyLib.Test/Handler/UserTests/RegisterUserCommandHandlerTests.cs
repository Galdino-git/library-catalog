using MyLib.Application.Handlers.Users.Commands.RegisterUser;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;
using MyLib.Application.Services;

namespace MyLib.Test.Handler.UserTests;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<RegisterUserCommandHandler>> _loggerMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<RegisterUserCommandHandler>>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _handler = new RegisterUserCommandHandler(_mockUnitOfWork.Object, _mapperMock.Object, _loggerMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_User_Id_When_Valid_Command()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "TestPass123!"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Email = command.Email,
            PasswordHash = string.Empty // Será setado pelo handler
        };

        _mockUnitOfWork.Setup(x => x.UserRepository.UsernameExistsAsync(command.Username)).ReturnsAsync(false);
        _mockUnitOfWork.Setup(x => x.UserRepository.EmailExistsAsync(command.Email)).ReturnsAsync(false);
        _mapperMock.Setup(x => x.Map<User>(command)).Returns(user);
        _passwordServiceMock.Setup(x => x.HashPassword(command.Password)).Returns("hashedpassword");
        _mockUnitOfWork.Setup(x => x.UserRepository.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        Assert.Equal(user.Id, result);
        Assert.Equal("hashedpassword", user.PasswordHash);

        _mockUnitOfWork.Verify(x => x.UserRepository.AddAsync(It.IsAny<User>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        _passwordServiceMock.Verify(x => x.HashPassword(command.Password), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Username_Already_Exists()
    {
        var command = new RegisterUserCommand
        {
            Username = "existinguser",
            Email = "test@example.com",
            Password = "TestPass123!"
        };

        _mockUnitOfWork.Setup(x => x.UserRepository.UsernameExistsAsync(command.Username)).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Username already exists.", exception.Message);

        _mockUnitOfWork.Verify(x => x.UserRepository.AddAsync(It.IsAny<User>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        _mockUnitOfWork.Verify(x => x.UserRepository.EmailExistsAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Email_Already_Exists()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "existing@example.com",
            Password = "TestPass123!"
        };

        _mockUnitOfWork.Setup(x => x.UserRepository.UsernameExistsAsync(command.Username)).ReturnsAsync(false);
        _mockUnitOfWork.Setup(x => x.UserRepository.EmailExistsAsync(command.Email)).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Email already exists.", exception.Message);

        _mockUnitOfWork.Verify(x => x.UserRepository.AddAsync(It.IsAny<User>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}