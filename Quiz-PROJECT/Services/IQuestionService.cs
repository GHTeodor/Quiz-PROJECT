using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDTO>> Get();
    Task<Question> GetById(int id);
    Task<Question> Post(CreateQuestionDTO question);
    Task<Question> Put(UpdateQuestionDTO person, int id);
    Task DeleteById(int id);
}