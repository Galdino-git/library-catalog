namespace MyLib.Application.Handlers.Users.DTOs
{
    public class PasswordResetRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public bool Used { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? UsedAt { get; set; }
    }
}