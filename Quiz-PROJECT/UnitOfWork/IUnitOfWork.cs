using Quiz_PROJECT.Models;
using Quiz_PROJECT.Repositories;

namespace Quiz_PROJECT.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    IUserRepository Users { get; }
    IRepository<Question> Questions { get; }
    Task SaveAsync();
}