using MyLib.Application.DTOs;
using MyLib.Application.Handlers.Users.Commands.LoginUser;
using MyLib.Application.Handlers.Users.Commands.RegisterUser;
using MyLib.Application.Handlers.Users.Commands.RequestPasswordReset;
using MyLib.Application.Handlers.Users.Commands.ResetPassword;
using MyLib.Application.Handlers.Users.DTOs;
using MyLib.Application.Handlers.Users.Queries.GetUserById;
using MyLib.Application.Handlers.Users.Queries.GetUsersList;
using MyLib.Presentation.Controllers;

namespace MyLib.Test.Controller;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new UsersController(_mockMediator.Object);
    }

    [Fact]
    public async Task Register_Should_Return_Ok_When_Successful()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "TestPass123!"
        };

        var expectedId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var result = await _controller.Register(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedId, okResult.Value);
    }

    [Fact]
    public async Task Register_Should_Return_BadRequest_When_Exception_Occurs()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "TestPass123!"
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApplicationException("User already exists"));

        var result = await _controller.Register(command);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Login_Should_Return_Ok_With_Token_When_Successful()
    {
        var command = new LoginUserCommand
        {
            UsernameOrEmail = "testuser",
            Password = "TestPass123!"
        };

        var expectedToken = "jwt-token";
        _mockMediator.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedToken);

        var result = await _controller.Login(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Login_Should_Return_BadRequest_When_Exception_Occurs()
    {
        var command = new LoginUserCommand
        {
            UsernameOrEmail = "testuser",
            Password = "wrongpassword"
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApplicationException("Invalid credentials"));

        var result = await _controller.Login(command);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task RequestPasswordReset_Should_Return_Ok_When_Successful()
    {
        var command = new RequestPasswordResetCommand
        {
            Email = "test@example.com"
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<RequestPasswordResetCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.RequestPasswordReset(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task ResetPassword_Should_Return_Ok_When_Successful()
    {
        var command = new ResetPasswordCommand
        {
            Token = "valid-token",
            NewPassword = "NewTestPass123!"
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.ResetPassword(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetById_Should_Return_Ok_When_User_Exists()
    {
        var userId = Guid.NewGuid();
        var expectedUser = new UserDetailsDto
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com"
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var result = await _controller.GetById(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedUser, okResult.Value);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        var userId = Guid.NewGuid();

        _mockMediator.Setup(x => x.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDetailsDto?)null);

        var result = await _controller.GetById(userId);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Exception_Occurs()
    {
        var userId = Guid.NewGuid();

        _mockMediator.Setup(x => x.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApplicationException("User not found"));

        var result = await _controller.GetById(userId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task GetUsers_Should_Return_Ok_When_Successful()
    {
        var query = new GetUsersListQuery
        {
            Page = 1,
            PageSize = 10
        };

        var expectedResult = new PaginatedResult<UserListDto>
        {
            Items = new List<UserListDto>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<GetUsersListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.GetUsers(query);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }
}