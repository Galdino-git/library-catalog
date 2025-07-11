using MyLib.Application.Handlers.Books.Commands.RegisterBook;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.BookTests
{
    public class RegisterBookCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RegisterBookCommandHandler>> _loggerMock;

        public RegisterBookCommandHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RegisterBookCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldRegisterBookAndReturnId()
        {
            var user = new User { Id = Guid.NewGuid(), Username = "user" };
            var book = new Book();

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<RegisterBookCommand>())).Returns(book);
            _unitOfWorkMock.Setup(u => u.BookRepository.AddAsync(book)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new RegisterBookCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new RegisterBookCommand { RegisteredByUserId = user.Id };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(book.Id, result);

            _unitOfWorkMock.Verify(u => u.BookRepository.AddAsync(book), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            _loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().ToLower().Contains("successfully")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenUserNotFound()
        {
            var book = new Book();

            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<RegisterBookCommand>())).Returns(book);
            _unitOfWorkMock.Setup(u => u.BookRepository.AddAsync(book)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            var handler = new RegisterBookCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new RegisterBookCommand { RegisteredByUserId = Guid.NewGuid() };

            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));

            _loggerMock.Verify(l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

            _unitOfWorkMock.Verify(u => u.BookRepository.AddAsync(book), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ThrowError_WhenFailToSave()
        {
            var user = new User { Id = Guid.NewGuid(), Username = "user" };
            var book = new Book();

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<RegisterBookCommand>())).Returns(book);
            _unitOfWorkMock.Setup(u => u.BookRepository.AddAsync(book)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(0);

            var handler = new RegisterBookCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new RegisterBookCommand { RegisteredByUserId = user.Id };

            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(u => u.BookRepository.AddAsync(book), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            _loggerMock.Verify(l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().ToLower().Contains("failed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }
    }
}
