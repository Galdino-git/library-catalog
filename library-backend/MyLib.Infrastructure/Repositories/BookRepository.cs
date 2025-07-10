using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLib.Domain.Entities;
using MyLib.Domain.Filter;
using MyLib.Domain.IRepositories;
using MyLib.Infrastructure.Data;

namespace MyLib.Infrastructure.Repositories
{
    public class BookRepository(BookCatalogDbContext context, ILogger logger) : BaseRepository<Book>(context, logger), IBookRepository
    {
        public async Task<IEnumerable<Book>> GetByFilterAsync(BookFilter filter, int pageNumber, int pageSize)
        {
            IQueryable<Book> query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(b => b.Title.Contains(filter.Title));

            if (!string.IsNullOrWhiteSpace(filter.Author))
                query = query.Where(b => b.Author.Contains(filter.Author));

            if (!string.IsNullOrWhiteSpace(filter.ISBN))
                query = query.Where(b => b.ISBN == filter.ISBN);

            if (filter.Gender != null && filter.Gender.Any())
                query = query.Where(b => filter.Gender.Contains(b.Gender));

            if (!string.IsNullOrWhiteSpace(filter.Publisher))
                query = query.Where(b => b.Publisher.Contains(filter.Publisher));

            if (filter.RegisteredByUserId.HasValue)
                query = query.Where(b => b.RegisteredByUserId == filter.RegisteredByUserId);


            try
            {
                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving books by filter in {REPO}", typeof(BookRepository));
                return [];
            }
        }
    }
}
