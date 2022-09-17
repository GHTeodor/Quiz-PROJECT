using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public interface IQuestionService
{
    Task<IEnumerable<Question>> Get();
    Task<Question> Post(Question question);
}