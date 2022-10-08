using AutoMapper;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class QuestionService : IQuestionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public QuestionService(DBContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
    }

    public async Task<IEnumerable<QuestionDTO>> GetAllAsync(CancellationToken token = default)
    {
        return _mapper.Map<IEnumerable<QuestionDTO>>(await _unitOfWork.Questions.GetAllAsync(token));
    }

    public async Task<Question> GetByIdAsync(long id, CancellationToken token = default)
    {
        return await _unitOfWork.Questions.GetByIdAsync(id, token);
    }

    public async Task<Question> CreateAsync(CreateQuestionDTO createAnswer, CancellationToken token = default)
    {
        Question question = _mapper.Map<Question>(createAnswer);

        question.UpdatedAt = null;
        
        await _unitOfWork.Questions.CreateAsync(question, token); 
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();

        return question;
    }
    
    public async Task<Question> UpdateByIdAsync(UpdateQuestionDTO question, long id, CancellationToken token = default)
    {
        Question questionForUpdate = _mapper.Map(question, await GetByIdAsync(id, token));

        questionForUpdate.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
        
        await _unitOfWork.Questions.UpdateAsync(questionForUpdate);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();

        return questionForUpdate;
    }

    public async Task DeleteByIdAsync(long id, CancellationToken token = default)
    {
        await _unitOfWork.Questions.DeleteByIdAsync(id, token);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();
    }
}