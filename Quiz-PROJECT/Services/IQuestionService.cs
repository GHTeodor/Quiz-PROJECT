using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDTO>> GetAllAsync();
    Task<Question> GetByIdAsync(long id);
    Task<Question> CreateAsync(CreateQuestionDTO question);
    Task<Question> UpdateByIdAsync(UpdateQuestionDTO person, long id);
    Task DeleteByIdAsync(long id);
}