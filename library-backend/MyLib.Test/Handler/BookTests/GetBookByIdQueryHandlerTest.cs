using MyLib.Application.DTOs;
using MyLib.Application.Handlers.Books.Queries.GetBookById;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.BookTests
{
    public class GetBookByIdQueryHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetBookByIdQueryHandler>> _loggerMock;

        public GetBookByIdQueryHandlerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetBookByIdQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnBookDetails_WhenBookExists()
        {
            var book = new Book { Id = Guid.NewGuid(), Title = "Test Book" };
            var bookDto = new BookDetailsDto { Id = book.Id, Title = book.Title };
            var query = new GetBookByIdQuery { Id = book.Id };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(book.Id)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDetailsDto>(book)).Returns(bookDto);

            var handler = new GetBookByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Title, result.Title);

            _mapperMock.Verify(m => m.Map<BookDetailsDto>(book), Times.Once);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().ToLower().Contains("successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenBookNotFound()
        {
            var id = Guid.NewGuid();
            var query = new GetBookByIdQuery { Id = id };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(id)).ReturnsAsync((Book?)null);

            var handler = new GetBookByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(query, CancellationToken.None));

            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
            _mapperMock.Verify(m => m.Map<BookDetailsDto>(It.IsAny<Book>()), Times.Never);
        }
    }
}
