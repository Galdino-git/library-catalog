using MediatR;
using MyLib.Application.DTOs;

namespace MyLib.Application.Handlers.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<BookDetailsDto>
    {
        public Guid Id { get; set; }
    }
}