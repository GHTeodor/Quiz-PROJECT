using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public class RefreshTokenRepository: IRefreshTokenRepository
{
    private readonly DBContext _dbContext;

    public RefreshTokenRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<RefreshToken?> GetByUserIdAsync(long userId, CancellationToken token = default)
    {
        return await _dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.UserId == userId, token);
    }
    
    public async Task CreateAsync(RefreshToken refreshToken, CancellationToken token = default)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, token);
    }
    
    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        await Task.FromResult(_dbContext.RefreshTokens.Update(refreshToken));
    }

    public async Task DeleteByUserIdAsync(long userId, CancellationToken token = default)
    {
        _dbContext.RefreshTokens.Remove(await GetByUserIdAsync(userId, token));
    }
}