using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public class QuestionRepository : IRepository<Question>
{
    private readonly DBContext _dbContext;

    public QuestionRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        return await _dbContext.Questions.Include(a => a.Answers).ToListAsync();
    }

    public async Task<Question> GetByIdAsync(int id)
    {
        var question = await _dbContext.Questions.SingleOrDefaultAsync(q=>q.Id == id);

        if (question is null)
            throw new NotFoundException("Question not exist", $"There is no question with Id: {id}");

        return question;
    }

    public async Task CreateAsync(Question question)
    {
        await _dbContext.Questions.AddAsync(question);
    }

    public async Task UpdateAsync(Question question)
    {
        _dbContext.Questions.Update(question);
    }

    public async Task DeleteByIdAsync(int id)
    {
        _dbContext.Questions.Remove(await GetByIdAsync(id));
    }
}