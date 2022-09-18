using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public class AnswersRepository : IRepository<Answers>
{
    private readonly DBContext _dbContext;

    public AnswersRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Answers>> GetAllAsync()
    {
        return await _dbContext.Answers.Include(a => a.Question).ToListAsync();
    }

    public async Task<Answers> GetByIdAsync(int id)
    {
        var answer = await _dbContext.Answers.SingleOrDefaultAsync(a => a.Id == id);

        if (answer is null)
            throw new NotFoundException("Answer not exist", $"There is no answer with Id: {id}");
        
        return answer;
    }

    public async Task CreateAsync(Answers answers)
    {
        await _dbContext.Answers.AddAsync(answers);
    }

    public async Task UpdateAsync(Answers answers)
    {
        _dbContext.Answers.Update(answers);
    }

    public async Task DeleteByIdAsync(int id)
    {
        _dbContext.Answers.Remove(await GetByIdAsync(id));
    }
}