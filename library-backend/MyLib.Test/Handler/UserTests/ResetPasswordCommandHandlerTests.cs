using MyLib.Application.Handlers.Users.Commands.ResetPassword;
using MyLib.Application.Services;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.UserTests;

public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly ResetPasswordCommandHandler _handler;

    public ResetPasswordCommandHandlerTests()
    {
        _handler = new ResetPasswordCommandHandler(_unitOfWorkMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Reset_Password_When_Token_Is_Valid()
    {
        var command = new ResetPasswordCommand { Token = "token", NewPassword = "NewPass123!" };
        var resetRequest = new PasswordResetRequest { Token = "token", Used = false, Expiration = DateTime.UtcNow.AddHours(1), UserId = Guid.NewGuid() };
        var user = new User { Id = resetRequest.UserId, PasswordHash = "oldhash" };

        _unitOfWorkMock.Setup(x => x.PasswordResetRepository.GetByTokenAsync(command.Token)).ReturnsAsync(resetRequest);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(resetRequest.UserId)).ReturnsAsync(user);
        _passwordServiceMock.Setup(x => x.HashPassword(command.NewPassword)).Returns("newhash");
        _unitOfWorkMock.Setup(x => x.UserRepository.UpdateAsync(user)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.PasswordResetRepository.UpdateAsync(resetRequest)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        await _handler.Handle(command, default);

        Assert.Equal("newhash", user.PasswordHash);
        Assert.True(resetRequest.Used);

        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.GetByTokenAsync(command.Token), Times.Once);
        _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(resetRequest.UserId), Times.Once);
        _passwordServiceMock.Verify(x => x.HashPassword(command.NewPassword), Times.Once);
        _unitOfWorkMock.Verify(x => x.UserRepository.UpdateAsync(user), Times.Once);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.UpdateAsync(resetRequest), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Token_Is_Invalid()
    {
        var command = new ResetPasswordCommand { Token = "invalid", NewPassword = "NewPass123!" };

        _unitOfWorkMock.Setup(x => x.PasswordResetRepository.GetByTokenAsync(command.Token)).ReturnsAsync((PasswordResetRequest?)null);

        await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, default));

        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.GetByTokenAsync(command.Token), Times.Once);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _passwordServiceMock.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.UserRepository.UpdateAsync(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.UpdateAsync(It.IsAny<PasswordResetRequest>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Token_Is_Expired()
    {
        var command = new ResetPasswordCommand { Token = "token", NewPassword = "NewPass123!" };
        var resetRequest = new PasswordResetRequest { Token = "token", Used = false, Expiration = DateTime.UtcNow.AddHours(-1), UserId = Guid.NewGuid() };

        _unitOfWorkMock.Setup(x => x.PasswordResetRepository.GetByTokenAsync(command.Token)).ReturnsAsync(resetRequest);

        await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, default));

        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.GetByTokenAsync(command.Token), Times.Once);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _passwordServiceMock.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.UserRepository.UpdateAsync(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.UpdateAsync(It.IsAny<PasswordResetRequest>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Token_Already_Used()
    {
        var command = new ResetPasswordCommand { Token = "token", NewPassword = "NewPass123!" };
        var resetRequest = new PasswordResetRequest { Token = "token", Used = true, Expiration = DateTime.UtcNow.AddHours(1), UserId = Guid.NewGuid() };

        _unitOfWorkMock.Setup(x => x.PasswordResetRepository.GetByTokenAsync(command.Token)).ReturnsAsync(resetRequest);

        await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, default));

        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.GetByTokenAsync(command.Token), Times.Once);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _passwordServiceMock.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.UserRepository.UpdateAsync(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.PasswordResetRepository.UpdateAsync(It.IsAny<PasswordResetRequest>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}