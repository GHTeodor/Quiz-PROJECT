using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return _dbContext.Users;
    }
    
    public async Task<User> GetByIdAsync(long id)
    {
        User user = await _dbContext.Users.Include(u => u.RefreshToken).SingleOrDefaultAsync(u => u.Id == id)
                    ?? throw new NotFoundException("User not exist", $"There is no user with Id: {id}");

        return user;
    }

    public async Task CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
    }

    public async Task DeleteByIdAsync(long id)
    {   
        _dbContext.Users.Remove(await GetByIdAsync(id));
    }
    
    public async Task<User> FindByEmailAsync(string email)
    {
        // User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper())
                    // ?? throw new NotFoundException("User not exist", $"There is no user with Eamil: {email}");
        
        return (await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper()))!;
    }

    public async Task<User> FindByPhoneAsync(string phone)
    {
        return (await _dbContext.Users.SingleOrDefaultAsync(u => u.Phone == phone))!; 
    }
}