using Microsoft.EntityFrameworkCore;
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
        return _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id).Result!;
    }
    
    public User Post(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChangesAsync();
        return user;
    }
    
    public User Put(User user, int id)
    {
        User userForUpdate = GetById(id);
        userForUpdate.Name = user.Name;
        userForUpdate.NickName = user.NickName;
        _dbContext.SaveChangesAsync();
        return userForUpdate;
    }
    
    public void DeleteById(int id)
    {
        _dbContext.Users.Remove(GetById(id));
        _dbContext.SaveChangesAsync();
    }
}