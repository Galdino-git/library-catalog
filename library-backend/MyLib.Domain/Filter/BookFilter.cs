namespace MyLib.Domain.Filter
{
    public class BookFilter
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public IEnumerable<string>? Gender { get; set; }
        public string? Publisher { get; set; }

        public Guid? RegisteredByUserId { get; set; }
    }
}