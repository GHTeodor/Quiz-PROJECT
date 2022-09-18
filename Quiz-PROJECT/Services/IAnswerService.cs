using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public interface IAnswerService
{
    Task<IEnumerable<Answers>> Get();
    Task<Answers> GetById(int id);
    Task<Answers> Post(Answers answer);
    Task<Answers> Put(Answers person, int id);
    Task DeleteById(int id);
}