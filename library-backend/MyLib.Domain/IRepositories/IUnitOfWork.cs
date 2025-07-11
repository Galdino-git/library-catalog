namespace MyLib.Domain.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IBookRepository BookRepository { get; }
        IPasswordResetRequestRepository PasswordResetRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
