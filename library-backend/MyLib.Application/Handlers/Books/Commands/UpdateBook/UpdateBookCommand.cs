using MediatR;

namespace MyLib.Application.Handlers.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public string Synopsis { get; set; } = string.Empty;
        public byte[]? CoverImage { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}