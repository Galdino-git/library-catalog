using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLib.Application.Handlers.Books.Commands.DeleteBook;
using MyLib.Application.Handlers.Books.Commands.RegisterBook;
using MyLib.Application.Handlers.Books.Commands.UpdateBook;
using MyLib.Application.Handlers.Books.Queries.GetBookById;
using MyLib.Application.Handlers.Books.Queries.GetBooksList;
using MyLib.Domain.Filter;

namespace MyLib.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Registers a new book in the system.
        /// </summary>
        /// <param name="command">The command containing the details of the book to be registered.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see cref="CreatedAtActionResult"/>
        /// if the operation is successful.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterBook([FromBody] RegisterBookCommand command)
        {
            var bookId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBookById), new { id = bookId }, null);
        }

        /// <summary>
        /// Gets a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>The book details if found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var query = new GetBookByIdQuery { Id = id };
            var book = await _mediator.Send(query);
            return Ok(book);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="command">The command containing the updated book details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var command = new DeleteBookCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Gets a paginated list of books with optional filtering.
        /// </summary>
        /// <param name="filter">Filter containing the books.</param>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <returns>A paginated list of books.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks(
            [FromQuery] BookFilter filter,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetBooksListQuery
            {
                Title = filter.Title,
                Author = filter.Author,
                ISBN = filter.ISBN,
                Gender = filter.Gender,
                Publisher = filter.Publisher,
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
