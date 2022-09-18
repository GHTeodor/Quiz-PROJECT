using System.Text.RegularExpressions;
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

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return _dbContext.Users;
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        
        if (user is null)
            throw new NotFoundException("User not exist", $"There is no user with Id: {id}");

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

    public async Task DeleteByIdAsync(int id)
    {   
        _dbContext.Users.Remove(await GetByIdAsync(id));
    }
    
    public async Task<User> FindByEmailAsync(string email)
    {
        return (await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper()))!;
    }
    
    public async Task<User> FindByPhoneAsync(string phone)
    {
        string phoneNumber = Regex.Replace(phone, "[^0-9]", ""); // only numbers
        return (await _dbContext.Users.SingleOrDefaultAsync(u => u.Phone == phone || u.Phone == phoneNumber))!;
    }
}