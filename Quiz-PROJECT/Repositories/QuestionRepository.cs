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
    
    public async Task<IEnumerable<Question>> GetAllAsync(CancellationToken token = default)
    {
        return await _dbContext.Questions.Include(a => a.IncorrectAnswers).ToListAsync(token);
    }

    public async Task<Question> GetByIdAsync(long id, CancellationToken token = default)
    {
        var question = await _dbContext.Questions.Include(a => a.IncorrectAnswers)
            .SingleOrDefaultAsync(q => q.Id == id, token);

        if (question is null)
            throw new NotFoundException("Question not exist", $"There is no question with Id: {id}");

        return question;
    }

    public async Task CreateAsync(Question question, CancellationToken token = default)
    {
        await _dbContext.Questions.AddAsync(question, token);

        if (question.IncorrectAnswers.Any())
        {
            foreach (var answer in question.IncorrectAnswers)
            {
                answer.UpdatedAt = null;
                await _dbContext.Answers.AddAsync(answer, token);
            }
        }
    }

    public async Task UpdateAsync(Question question)
    {
        if (question.IncorrectAnswers.Any())
        {
            // foreach (var answer in question.Answers)
            // {
            //     _dbContext.Entry(answer).State = EntityState.Added;
            // }
            foreach (var answer in question.IncorrectAnswers)
            {
                if (answer.Id == default) continue;
                var answerModel = new Answer
                {
                    Id = answer.Id,
                    IncorrectAnswer = answer.IncorrectAnswer,
                    UpdatedAt = DateTimeOffset.Now.ToLocalTime()
                };
                _dbContext.Answers.Attach(answerModel);
                _dbContext.Entry(answerModel).Property(t => t.IncorrectAnswer).IsModified = true;
            }
        }

        await Task.FromResult(_dbContext.Questions.Update(question));
    }

    public async Task DeleteByIdAsync(long id, CancellationToken token = default)
    {
        _dbContext.Questions.Remove(await GetByIdAsync(id, token));
    }
}