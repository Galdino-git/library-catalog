using MediatR;
using MyLib.Application.DTOs;

namespace MyLib.Application.Handlers.Books.Queries.GetBooksList
{
    public class GetBooksListQuery : IRequest<PaginatedResult<BookDetailsDto>>
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public IEnumerable<string>? Gender { get; set; }
        public string? Publisher { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}