using MyLib.Domain.Entities;

namespace MyLib.Domain.IRepositories
{
    public interface IPasswordResetRequestRepository : IBaseRepository<PasswordResetRequest>
    {
        Task<PasswordResetRequest?> GetByTokenAsync(string token);
        Task<IEnumerable<PasswordResetRequest>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<PasswordResetRequest>> GetUnusedByUserIdAsync(Guid userId);
    }
}