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

    public async Task<IEnumerable<QuestionDTO>> Get()
    {
        return _mapper.Map<IEnumerable<QuestionDTO>>(await _unitOfWork.Questions.GetAllAsync());
    }

    public async Task<Question> GetById(int id)
    {
        return await _unitOfWork.Questions.GetByIdAsync(id);
    }

    public async Task<Question> Post(CreateQuestionDTO createAnswer)
    {
        Question question = _mapper.Map<Question>(createAnswer);

        question.CreatedAt = DateTimeOffset.Now.ToLocalTime();
        question.UpdatedAt = null;
        
        await _unitOfWork.Questions.CreateAsync(question); 
        await _unitOfWork.SaveAsync();

        return question;
    }
    
    public async Task<Question> Put(UpdateQuestionDTO question, int id)
    {
        Question questionForUpdate = _mapper.Map(question, await GetById(id));

        questionForUpdate.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
        
        await _unitOfWork.Questions.UpdateAsync(questionForUpdate);
        await _unitOfWork.SaveAsync();

        return questionForUpdate;
    }

    public async Task DeleteById(int id)
    {
        await _unitOfWork.Questions.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }
}