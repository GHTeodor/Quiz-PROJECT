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

    public async Task<IEnumerable<QuestionDTO>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<QuestionDTO>>(await _unitOfWork.Questions.GetAllAsync());
    }

    public async Task<Question> GetByIdAsync(long id)
    {
        return await _unitOfWork.Questions.GetByIdAsync(id);
    }

    public async Task<Question> CreateAsync(CreateQuestionDTO createAnswer)
    {
        Question question = _mapper.Map<Question>(createAnswer);

        question.UpdatedAt = null;
        
        await _unitOfWork.Questions.CreateAsync(question); 
        await _unitOfWork.SaveAsync();
        await _unitOfWork.DisposeAsync();

        return question;
    }
    
    public async Task<Question> UpdateByIdAsync(UpdateQuestionDTO question, long id)
    {
        Question questionForUpdate = _mapper.Map(question, await GetByIdAsync(id));

        questionForUpdate.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
        
        await _unitOfWork.Questions.UpdateAsync(questionForUpdate);
        await _unitOfWork.SaveAsync();
        await _unitOfWork.DisposeAsync();

        return questionForUpdate;
    }

    public async Task DeleteByIdAsync(long id)
    {
        await _unitOfWork.Questions.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        await _unitOfWork.DisposeAsync();
    }
}