using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;
using MyLib.Infrastructure.Data;

namespace MyLib.Infrastructure.Repositories
{
    public class UserRepository(BookCatalogDbContext context, ILogger logger) : BaseRepository<User>(context, logger), IUserRepository
    {
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}