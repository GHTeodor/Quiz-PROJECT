using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class AdminService: IAdminService
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Dictionary<string, List<string>>> GetQuizChartsAsync(CancellationToken token = default)
    {
        return new Dictionary<string, List<string>>
            {
                {"category", await _unitOfWork.Questions.GetChartInfoByCategory(token)},
                {"difficulty", await _unitOfWork.Questions.GetChartInfoByDifficulty(token)},
                {"type", await _unitOfWork.Questions.GetChartInfoByType(token)}
            };
    }
}

