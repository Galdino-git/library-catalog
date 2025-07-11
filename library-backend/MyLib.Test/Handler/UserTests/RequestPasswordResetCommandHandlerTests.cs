using Microsoft.Extensions.Configuration;
using MyLib.Application.Handlers.Users.Commands.RequestPasswordReset;
using MyLib.Application.Services;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.UserTests;

public class RequestPasswordResetCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly RequestPasswordResetCommandHandler _handler;

    public RequestPasswordResetCommandHandlerTests()
    {
        _configurationMock.Setup(x => x["Frontend:ResetPasswordUrl"]).Returns("http://localhost/reset-password");
        _handler = new RequestPasswordResetCommandHandler(
            _unitOfWorkMock.Object,
            _emailServiceMock.Object,
            _configurationMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Create_Reset_Request_And_Send_Email_When_Email_Exists()
    {
        var command = new RequestPasswordResetCommand { Email = "test@example.com" };
        var user = new User { Id = Guid.NewGuid(), Email = command.Email };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByEmailAsync(command.Email)).ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.PasswordResetRepository.AddAsync(It.IsAny<PasswordResetRequest>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        _emailServiceMock.Setup(x => x.SendEmailAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        await _handler.Handle(command, default);

        _unitOfWorkMock.Verify(x => x.UserRepository.GetByEmailAsync(command.Email), Times.Once);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.AddAsync(It.IsAny<PasswordResetRequest>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _emailServiceMock.Verify(x => x.SendEmailAsync(
            user.Email, It.IsAny<string>(), It.Is<string>(body => body.Contains("reset-password"))), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Not_Throw_Or_Send_Email_When_Email_Does_Not_Exist()
    {
        var command = new RequestPasswordResetCommand { Email = "notfound@example.com" };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByEmailAsync(command.Email)).ReturnsAsync((User?)null);

        await _handler.Handle(command, default);

        _unitOfWorkMock.Verify(x => x.UserRepository.GetByEmailAsync(command.Email), Times.Once);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.AddAsync(It.IsAny<PasswordResetRequest>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}