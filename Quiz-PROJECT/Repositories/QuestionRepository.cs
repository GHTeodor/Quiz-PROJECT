using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        return await _dbContext.Questions.Include(a => a.IncorrectAnswers).ToListAsync();
    }

    public async Task<Question> GetByIdAsync(long id)
    {
        var question = await _dbContext.Questions.Include(a => a.IncorrectAnswers)
            .SingleOrDefaultAsync(q => q.Id == id);

        if (question is null)
            throw new NotFoundException("Question not exist", $"There is no question with Id: {id}");

        return question;
    }

    public async Task CreateAsync(Question question)
    {
        await _dbContext.Questions.AddAsync(question);

        if (question.IncorrectAnswers.Any())
        {
            foreach (var answer in question.IncorrectAnswers)
            {
                await _dbContext.Answers.AddAsync(answer);
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
                    IncorrectAnswer = answer.IncorrectAnswer
                };
                _dbContext.Answers.Attach(answerModel);
                _dbContext.Entry(answerModel).Property(t => t.IncorrectAnswer).IsModified = true;
            }
        }

        _dbContext.Questions.Update(question);
    }

    public async Task DeleteByIdAsync(long id)
    {
        _dbContext.Questions.Remove(await GetByIdAsync(id));
    }
}