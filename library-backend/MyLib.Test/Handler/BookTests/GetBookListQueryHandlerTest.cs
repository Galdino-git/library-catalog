using MyLib.Application.DTOs;
using MyLib.Application.Handlers.Books.Queries.GetBooksList;
using MyLib.Domain.Entities;
using MyLib.Domain.Filter;
using MyLib.Domain.IRepositories;

namespace MyLib.Test.Handler.BookTests
{
    public class GetBookListQueryHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetBooksListQueryHandler>> _loggerMock;

        public GetBookListQueryHandlerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetBooksListQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedResult_WhenBooksExist()
        {
            var books = new List<Book> { new() { Id = Guid.NewGuid(), Title = "Book 1" } };
            var bookDtos = new List<BookDetailsDto> { new() { Id = books[0].Id, Title = books[0].Title } };
            var query = new GetBooksListQuery { Page = 1, PageSize = 10 };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByFilterAsync(It.IsAny<BookFilter>(), 1, 10)).ReturnsAsync((1, books));
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDetailsDto>>(books)).Returns(bookDtos);

            var handler = new GetBooksListQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(bookDtos[0].Id, result.Items[0].Id);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().ToLower().Contains("showing")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyPaginatedResult_WhenNoBooksExist()
        {
            var books = new List<Book>();
            var bookDtos = new List<BookDetailsDto>();
            var query = new GetBooksListQuery { Page = 1, PageSize = 10 };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByFilterAsync(It.IsAny<BookFilter>(), 1, 10)).ReturnsAsync((0, books));
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDetailsDto>>(books)).Returns(bookDtos);

            var handler = new GetBooksListQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result.Items);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().ToLower().Contains("showing")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }
    }
}
