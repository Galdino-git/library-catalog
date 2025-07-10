using Microsoft.Extensions.Logging;
using MyLib.Domain.IRepositories;
using MyLib.Infrastructure.Data;

namespace MyLib.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BookCatalogDbContext _context;
        private readonly ILogger _logger;

        public IUserRepository UserRepository { get; private set; }
        public IBookRepository BookRepository { get; private set; }

        public UnitOfWork(BookCatalogDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory.CreateLogger("logs");

            UserRepository = new UserRepository(_context, _logger);
            BookRepository = new BookRepository(_context, _logger);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database.");
                throw;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
