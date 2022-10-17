using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public interface IQuestionRepository: IRepository<Question>
{
    public Task<List<string>> GetChartInfoByCategory(CancellationToken token = default);
    public Task<List<string>> GetChartInfoByDifficulty(CancellationToken token = default);
    public Task<List<string>> GetChartInfoByType(CancellationToken token = default);
}