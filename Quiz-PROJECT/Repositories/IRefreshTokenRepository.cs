using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByUserIdAsync(long userId, CancellationToken token = default);
    Task CreateAsync(RefreshToken refreshToken, CancellationToken token = default);
    Task UpdateAsync(RefreshToken refreshToken);
    Task DeleteByUserIdAsync(long userId, CancellationToken token = default);

}