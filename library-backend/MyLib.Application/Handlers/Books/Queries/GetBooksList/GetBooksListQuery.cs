using MyLib.Application.DTOs;
using MyLib.Domain.Filter;

namespace MyLib.Application.Handlers.Books.Queries.GetBooksList
{
    public class GetBooksListQuery : IRequest<PaginatedResult<BookDetailsDto>>
    {
        public BookFilter Filter { get; set; } = new();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}