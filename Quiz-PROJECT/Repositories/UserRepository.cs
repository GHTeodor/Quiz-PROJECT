using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DBContext _dbContext;

    public UserRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default)
    {
        return await _dbContext.Users.ToListAsync(token);
    }
    
    public async Task<User> GetByIdAsync(long id, CancellationToken token = default)
    {
        return await _dbContext.Users.Include(u => u.RefreshToken).SingleOrDefaultAsync(u => u.Id == id, token)
               ?? throw new NotFoundException("User not exist", $"There is no user with Id: {id}");;
    }

    public async Task CreateAsync(User user, CancellationToken token = default)
    {
        await _dbContext.Users.AddAsync(user, token);
    }

    public async Task UpdateAsync(User user)
    {
        await Task.FromResult(_dbContext.Users.Update(user));
    }

    public async Task DeleteByIdAsync(long id, CancellationToken token = default)
    {   
        _dbContext.Users.Remove(await GetByIdAsync(id, token));
    }
    
    public async Task<User> FindByEmailAsync(string email, CancellationToken token = default)
    {
        // User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper())
                    // ?? throw new NotFoundException("User not exist", $"There is no user with Eamil: {email}");
        
        return (await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper(), token))!;
    }

    public async Task<User> FindByPhoneAsync(string phone, CancellationToken token = default)
    {
        return (await _dbContext.Users.SingleOrDefaultAsync(u => u.Phone == phone, token))!; 
    }
}