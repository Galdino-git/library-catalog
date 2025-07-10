using MyLib.Domain.Entities;

namespace MyLib.Domain.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}
