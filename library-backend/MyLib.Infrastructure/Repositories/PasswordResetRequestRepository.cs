using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;
using MyLib.Infrastructure.Data;

namespace MyLib.Infrastructure.Repositories
{
    public class PasswordResetRequestRepository(BookCatalogDbContext context, ILogger logger) : BaseRepository<PasswordResetRequest>(context, logger), IPasswordResetRequestRepository
    {
        public async Task<PasswordResetRequest?> GetByTokenAsync(string token)
        {
            return await _context.PasswordResetRequests.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task<IEnumerable<PasswordResetRequest>> GetByUserIdAsync(Guid userId)
        {
            return await _context.PasswordResetRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PasswordResetRequest>> GetUnusedByUserIdAsync(Guid userId)
        {
            return await _context.PasswordResetRequests
                .Where(r => r.UserId == userId && !r.Used && r.Expiration > DateTime.UtcNow)
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();
        }
    }
}