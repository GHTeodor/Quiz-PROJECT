using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IAnswerService
{
    Task<IEnumerable<AnswerDTO>> Get();
    Task<Answer> GetById(int id);
    Task<Answer> Post(CreateAnswerDTO answer);
    Task<Answer> Put(UpdateAnswerDTO person, int id);
    Task DeleteById(int id);
}