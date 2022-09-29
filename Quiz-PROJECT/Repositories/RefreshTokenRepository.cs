using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public class RefreshTokenRepository: IRefreshTokenRepository
{
    private readonly DBContext _dbContext;

    public RefreshTokenRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<RefreshToken?> GetByUserIdAsync(long userId)
    {
        return await _dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.UserId == userId);
    }
    
    public async Task CreateAsync(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }
    
    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        _dbContext.RefreshTokens.Remove(await GetByUserIdAsync(userId));
    }
}