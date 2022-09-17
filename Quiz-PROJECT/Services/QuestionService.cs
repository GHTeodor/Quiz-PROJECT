using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public class QuestionService : IQuestionService
{
    private readonly DBContext _dbContext;
    private readonly IMapper _mapper;

    public QuestionService(DBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Question>> Get()
    {
        return await _dbContext.Question.Include(a => a.Answers).ToListAsync();
    }

    public async Task<Question> Post(Question question)
    {
        question.CreatedAt = DateTimeOffset.Now.ToLocalTime();
        question.UpdatedAt = null;
        await _dbContext.Question.AddAsync(question); 
        await _dbContext.SaveChangesAsync();

        return question;
    }
}