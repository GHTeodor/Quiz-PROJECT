using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDTO>> GetAllAsync(CancellationToken token = default);
    Task<Question> GetByIdAsync(long id, CancellationToken token = default);
    Task<Question> CreateAsync(CreateQuestionDTO question, CancellationToken token = default);
    Task<Question> UpdateByIdAsync(UpdateQuestionDTO person, long id, CancellationToken token = default);
    Task DeleteByIdAsync(long id, CancellationToken token = default);
}