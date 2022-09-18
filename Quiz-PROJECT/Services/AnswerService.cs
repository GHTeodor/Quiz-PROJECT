using AutoMapper;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
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

    public async Task<IEnumerable<AnswerDTO>> Get()
    {
        return _mapper.Map<IEnumerable<AnswerDTO>>(await _unitOfWork.Answers.GetAllAsync());
    }

    public async Task<Answer> GetById(int id)
    {
        return await _unitOfWork.Answers.GetByIdAsync(id);
    }
    
    public async Task<Answer> Post(CreateAnswerDTO createAnswer)
    {
        Answer answer = _mapper.Map<Answer>(createAnswer);

        answer.CreatedAt = DateTimeOffset.Now.ToLocalTime();
        answer.UpdatedAt = null;
        
        await _unitOfWork.Answers.CreateAsync(answer);
        await _unitOfWork.SaveAsync();
        
        return answer;
    }

    public async Task<Answer> Put(UpdateAnswerDTO answer, int id)
    {
        Answer answerForUpdate = _mapper.Map(answer, await GetById(id));

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