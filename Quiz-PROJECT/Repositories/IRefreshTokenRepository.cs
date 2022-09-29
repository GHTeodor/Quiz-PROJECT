using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByUserIdAsync(long userId);
    Task CreateAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task DeleteByUserIdAsync(long userId);

}