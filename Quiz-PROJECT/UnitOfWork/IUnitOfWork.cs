using Quiz_PROJECT.Models;
using Quiz_PROJECT.Repositories;

namespace Quiz_PROJECT.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    IUserRepository Users { get; }
    IQuestionRepository Questions { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    Task SaveAsync(CancellationToken token = default);
}