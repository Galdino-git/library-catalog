namespace MyLib.Domain.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IBookRepository BookRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
