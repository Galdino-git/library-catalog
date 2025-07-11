using MyLib.Application.Handlers.Users.DTOs;
using MyLib.Application.Handlers.Users.Queries.GetUsersList;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.UserTests;

public class GetUsersListQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetUsersListQueryHandler _handler;

    public GetUsersListQueryHandlerTests()
    {
        _handler = new GetUsersListQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Paginated_Users()
    {
        var users = new List<User> { new User { Id = Guid.NewGuid(), Username = "user1", Email = "u1@test.com" } };
        var query = new GetUsersListQuery { Page = 1, PageSize = 10 };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetAllAsync(1, 10)).ReturnsAsync(users);
        _mapperMock.Setup(x => x.Map<List<UserListDto>>(users)).Returns([new UserListDto { Username = "user1", Email = "u1@test.com" }]);

        var result = await _handler.Handle(query, default);

        Assert.NotNull(result);
        Assert.Single(result.Items);

        _unitOfWorkMock.Verify(x => x.UserRepository.GetAllAsync(1, 10), Times.Once);
        _mapperMock.Verify(x => x.Map<List<UserListDto>>(users), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_When_No_Users()
    {
        var users = new List<User>();
        var query = new GetUsersListQuery { Page = 1, PageSize = 10 };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetAllAsync(1, 10)).ReturnsAsync(users);
        _mapperMock.Setup(x => x.Map<List<UserListDto>>(users)).Returns([]);

        var result = await _handler.Handle(query, default);

        Assert.NotNull(result);
        Assert.Empty(result.Items);

        _unitOfWorkMock.Verify(x => x.UserRepository.GetAllAsync(1, 10), Times.Once);
        _mapperMock.Verify(x => x.Map<List<UserListDto>>(users), Times.Once);
    }
}