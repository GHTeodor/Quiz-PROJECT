using AutoMapper;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class AnswerService : IAnswerService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AnswerService(DBContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
    }

    public async Task<IEnumerable<Answers>> Get()
    {
        return await _unitOfWork.Answers.GetAllAsync();
    }

    public async Task<Answers> GetById(int id)
    {
        return await _unitOfWork.Answers.GetByIdAsync(id);
    }
    
    public async Task<Answers> Post(Answers answer)
    {
        answer.CreatedAt = DateTimeOffset.Now.ToLocalTime();
        answer.UpdatedAt = null;
        
        await _unitOfWork.Answers.CreateAsync(answer);
        await _unitOfWork.SaveAsync();
        
        return answer;
    }

    public async Task<Answers> Put(Answers answer, int id)
    {
        Answers answerForUpdate = await GetById(id);

        answerForUpdate.Answer_num = answer.Answer_num;
        answerForUpdate.QuestionId = answer.QuestionId;

        answerForUpdate.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
        
        await _unitOfWork.Answers.UpdateAsync(answerForUpdate);
        await _unitOfWork.SaveAsync();
        
        return answerForUpdate;
    }

    public async Task DeleteById(int id)
    {
        await _unitOfWork.Answers.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }
}