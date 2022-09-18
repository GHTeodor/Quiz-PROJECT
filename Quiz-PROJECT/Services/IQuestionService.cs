using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public interface IQuestionService
{
    Task<IEnumerable<Question>> Get();
    Task<Question> GetById(int id);
    Task<Question> Post(Question question);
    Task<Question> Put(Question person, int id);
    Task DeleteById(int id);
}