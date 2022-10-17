namespace Quiz_PROJECT.Services;

public interface IAdminService
{
    Task<Dictionary<string, List<string>>> GetQuizChartsAsync(CancellationToken token = default);
}