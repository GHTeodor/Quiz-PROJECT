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
        Answers = new AnswersRepository(_dbContext);
    }

    public IUserRepository Users { get; }
    public IRepository<Question> Questions { get; }
    public IRepository<Answers> Answers { get; }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
    
    private bool disposed = false;
 
    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            this.disposed = true;
        }
    }
 
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}