using MyLib.Application.Handlers.Users.DTOs;
using MyLib.Application.Handlers.Users.Queries.GetUserById;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.UserTests;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _handler = new GetUserByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_User_When_Found()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Username = "testuser", Email = "test@example.com" };
        var dto = new UserDetailsDto { Id = userId, Username = "testuser", Email = "test@example.com" };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserDetailsDto>(user)).Returns(dto);

        var result = await _handler.Handle(new GetUserByIdQuery { Id = userId }, default);

        Assert.NotNull(result);
        Assert.Equal(dto.Id, result.Id);

        _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDetailsDto>(user), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_User_Not_Found()
    {
        var userId = Guid.NewGuid();

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var result = await _handler.Handle(new GetUserByIdQuery { Id = userId }, default);

        Assert.Null(result);

        _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDetailsDto>(It.IsAny<User>()), Times.Never);
    }
}