using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public class AnswerService : IAnswerService
{
    private readonly DBContext _dbContext;
    private readonly IMapper _mapper;

    public AnswerService(DBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Answers>> Get()
    {
        return await _dbContext.Answers.Include(a => a.Question).ToListAsync();
    }

    public async Task<Answers> Post(Answers answer)
    {
        answer.CreatedAt = DateTimeOffset.Now.ToLocalTime();
        answer.UpdatedAt = null;
        
        await _dbContext.Answers.AddAsync(answer);
        await _dbContext.SaveChangesAsync();
        
        return answer;
    }
}