using MyLib.Application.Handlers.Books.Commands.UpdateBook;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.BookTests
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateBookCommandHandler>> _loggerMock;

        public UpdateBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateBookCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldUpdateBookSuccessfully()
        {
            var book = new Book { Id = Guid.NewGuid() };
            var command = new UpdateBookCommand { Id = book.Id, Title = "Updated" };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(book.Id)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.BookRepository.UpdateAsync(book)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map(command, book)).Verifiable();

            var handler = new UpdateBookCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            await handler.Handle(command, CancellationToken.None);

            _mapperMock.Verify(m => m.Map(command, book), Times.Once);
            _unitOfWorkMock.Verify(u => u.BookRepository.UpdateAsync(book), Times.Once);
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
        public async Task Handle_ShouldFail_WhenBookNotFound()
        {
            var command = new UpdateBookCommand { Id = Guid.NewGuid(), Title = "Updated" };
            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(command.Id)).ReturnsAsync((Book?)null);

            var handler = new UpdateBookCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));

            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

            _unitOfWorkMock.Verify(u => u.BookRepository.UpdateAsync(It.IsAny<Book>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ThrowError_WhenFailToSave()
        {
            var book = new Book { Id = Guid.NewGuid() };
            var command = new UpdateBookCommand { Id = book.Id, Title = "Updated" };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(book.Id)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.BookRepository.UpdateAsync(book)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(0);
            _mapperMock.Setup(m => m.Map(command, book)).Verifiable();

            var handler = new UpdateBookCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));

            _mapperMock.Verify(m => m.Map(command, book), Times.Once);
            _unitOfWorkMock.Verify(u => u.BookRepository.UpdateAsync(book), Times.Once);
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
