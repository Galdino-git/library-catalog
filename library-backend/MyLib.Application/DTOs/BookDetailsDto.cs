namespace MyLib.Application.DTOs
{
    public class BookDetailsDto
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

        public Guid? RegisteredByUserId { get; set; }
        public string? RegisteredByUserName { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
