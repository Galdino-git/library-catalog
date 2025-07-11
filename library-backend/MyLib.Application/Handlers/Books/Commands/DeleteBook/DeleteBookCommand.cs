using MediatR;

namespace MyLib.Application.Handlers.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 