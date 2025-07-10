using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLib.Domain.IRepositories;
using MyLib.Infrastructure.Data;

namespace MyLib.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly BookCatalogDbContext _context;
        protected readonly ILogger _logger;

        protected BaseRepository(BookCatalogDbContext context, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<T>().Remove(entity);
        }
    }
}
