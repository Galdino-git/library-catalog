using System;

namespace MyLib.Domain.Entities
{
    public class PasswordResetRequest : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public bool Used { get; set; } = false;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UsedAt { get; set; }
    }
} 