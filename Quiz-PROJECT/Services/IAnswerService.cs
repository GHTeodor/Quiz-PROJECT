using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public interface IAnswerService
{
    Task<IEnumerable<Answers>> Get();
    Task<Answers> Post(Answers answer);
}