using System.Text.Json;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Repositories;

namespace Quiz_PROJECT.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DBContext _dbContext;

    public UnitOfWork(DBContext dbContext)
    {
        _dbContext = dbContext;
        Users = new UserRepository(_dbContext);
        Questions = new QuestionRepository(_dbContext);
    }

    public IUserRepository Users { get; }
    public IRepository<Question> Questions { get; }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    private Utf8JsonWriter? _jsonWriter = new(new MemoryStream());

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _jsonWriter?.Dispose();
            _jsonWriter = null;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_jsonWriter is not null)
        {
            await _jsonWriter.DisposeAsync().ConfigureAwait(false);
        }

        _jsonWriter = null;
    }
}