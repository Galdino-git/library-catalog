using MyLib.Application.Handlers.Books.Commands.DeleteBook;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.BookTests
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteBookCommandHandler>> _loggerMock;

        public DeleteBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteBookCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldDeleteSuccesfully()
        {
            var book = new Book { Id = Guid.NewGuid() };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(book.Id)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new DeleteBookCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var command = new DeleteBookCommand { Id = book.Id };

            await handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.BookRepository.DeleteAsync(book.Id), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            _loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("successfully")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenBookNotFound()
        {
            var book = new Book { Id = Guid.NewGuid() };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(book.Id)).ReturnsAsync((Book?)null);

            var handler = new DeleteBookCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var command = new DeleteBookCommand { Id = book.Id };

            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));

            _loggerMock.Verify(l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

            _unitOfWorkMock.Verify(u => u.BookRepository.DeleteAsync(book.Id), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ThrowError_WhenFailToDelete()
        {
            var book = new Book { Id = Guid.NewGuid() };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(book.Id)).ReturnsAsync(book);

            var handler = new DeleteBookCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var command = new DeleteBookCommand { Id = book.Id };

            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));

            _loggerMock.Verify(l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().ToLower().Contains("failed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

            _unitOfWorkMock.Verify(u => u.BookRepository.DeleteAsync(book.Id), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}