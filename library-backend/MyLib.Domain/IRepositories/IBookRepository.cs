using MyLib.Domain.Entities;
using MyLib.Domain.Filter;

namespace MyLib.Domain.IRepositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<IEnumerable<Book>> GetByFilterAsync(BookFilter filter, int pageNumber, int pageSize);
    }
}