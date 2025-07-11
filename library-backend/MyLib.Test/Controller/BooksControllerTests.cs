using MyLib.Application.DTOs;
using MyLib.Application.Handlers.Books.Commands.DeleteBook;
using MyLib.Application.Handlers.Books.Commands.RegisterBook;
using MyLib.Application.Handlers.Books.Commands.UpdateBook;
using MyLib.Application.Handlers.Books.Queries.GetBookById;
using MyLib.Application.Handlers.Books.Queries.GetBooksList;
using MyLib.Domain.Filter;
using MyLib.Presentation.Controllers;

namespace MyLib.Test.Controller
{
    public class BooksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task RegisterBook_ReturnsCreatedAtAction()
        {
            var command = new RegisterBookCommand();
            var bookId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterBookCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(bookId);

            var result = await _controller.RegisterBook(command);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetBookById), createdAt.ActionName);
            Assert.Equal(bookId, createdAt.RouteValues["id"]);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkWithBook()
        {
            var id = Guid.NewGuid();
            var bookDto = new BookDetailsDto();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetBookByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(bookDto);

            var result = await _controller.GetBookById(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(bookDto, ok.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContent()
        {
            var id = Guid.NewGuid();
            var command = new UpdateBookCommand();
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()));

            var result = await _controller.UpdateBook(id, command);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(id, command.Id);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent()
        {
            var id = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBookCommand>(), It.IsAny<CancellationToken>()));

            var result = await _controller.DeleteBook(id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetBooks_ReturnsOkWithResult()
        {
            var filter = new BookFilter();
            var resultObj = new PaginatedResult<BookDetailsDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetBooksListQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(resultObj);

            var result = await _controller.GetBooks(filter, 1, 10);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(resultObj, ok.Value);
        }
    }
}
