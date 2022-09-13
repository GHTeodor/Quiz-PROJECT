using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public class UserService : IUserService
{
    DBContext _dbContext;

    public UserService(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<User> Get()
    {
        return _dbContext.Users;
    }
    
    public User GetById(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        
        if (user == null)
            throw new NotFoundException("User not exist", $"There is no user with Id: {id}");

        return user;
    }
    
    public async Task<User> Post(User user)
    {
        if (user == null)
        {
            throw new BadRequestException("Wrong field(s)",
                $"User's fields: {JsonConvert.SerializeObject(typeof(User).GetProperties().Select(f => f.Name))}");
        }

        user.Role = Role.USER;
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
    
    public async Task<User> Put(User user, int id)
    {
        User userForUpdate = GetById(id);

        userForUpdate.Name = user.Name;
        userForUpdate.NickName = user.NickName;
        userForUpdate.Email = user.Email;
        userForUpdate.Phone = user.Phone;
        userForUpdate.Age = user.Age;
        userForUpdate.Role = user.Role;
        userForUpdate.Password = user.Password;
        userForUpdate.ConfirmPassword = user.ConfirmPassword;
        
        await _dbContext.SaveChangesAsync();
        return userForUpdate;
    }
    
    public void DeleteById(int id)
    {
        _dbContext.Users.Remove(GetById(id));
        _dbContext.SaveChanges();
    }
}