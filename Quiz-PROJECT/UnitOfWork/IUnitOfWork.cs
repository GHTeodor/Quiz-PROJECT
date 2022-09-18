using Quiz_PROJECT.Models;
using Quiz_PROJECT.Repositories;

namespace Quiz_PROJECT.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRepository<Question> Questions { get; }
    IRepository<Answer> Answers { get; }
    Task SaveAsync();
}