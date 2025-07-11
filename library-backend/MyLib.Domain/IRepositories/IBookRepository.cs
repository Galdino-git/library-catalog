using MyLib.Domain.Entities;
using MyLib.Domain.Filter;

namespace MyLib.Domain.IRepositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        /// <summary>
        /// Returns the total filtered count and the paginated list
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<(int, IEnumerable<Book>)> GetByFilterAsync(BookFilter filter, int pageNumber, int pageSize);
    }
}