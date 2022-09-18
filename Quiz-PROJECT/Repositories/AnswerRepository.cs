using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public class AnswerRepository : IRepository<Answer>
{
    private readonly DBContext _dbContext;

    public AnswerRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Answer>> GetAllAsync()
    {
        return await _dbContext.Answers.Include(a => a.Question).ToListAsync();
    }

    public async Task<Answer> GetByIdAsync(int id)
    {
        var answer = await _dbContext.Answers.SingleOrDefaultAsync(a => a.Id == id);

        if (answer is null)
            throw new NotFoundException("Answer not exist", $"There is no answer with Id: {id}");
        
        return answer;
    }

    public async Task CreateAsync(Answer answer)
    {
        await _dbContext.Answers.AddAsync(answer);
    }

    public async Task UpdateAsync(Answer answer)
    {
        _dbContext.Answers.Update(answer);
    }

    public async Task DeleteByIdAsync(int id)
    {
        _dbContext.Answers.Remove(await GetByIdAsync(id));
    }
}