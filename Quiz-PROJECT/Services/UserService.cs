using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public class UserService : IUserService
{
    private readonly DBContext _dbContext;
    private readonly IMapper _mapper;

    public UserService(DBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public IEnumerable<UserDTO> Get()
    {
        var users =  _mapper.Map<IEnumerable<UserDTO>>(_dbContext.Users);
        return users;
    }
    
    public async Task<User> GetById(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            throw new NotFoundException("User not exist", $"There is no user with Id: {id}");

        return user;
    }
    
    public async Task<User> Post(CreateUserDTO createdUser)
    {
        if (createdUser == null)
        {
            throw new BadRequestException("Wrong field(s)",
                $"User's fields: {JsonConvert.SerializeObject(typeof(User).GetProperties().Select(f => f.Name))}");
        }

        var user = _mapper.Map<User>(createdUser);
        
        user.CreatedAt = DateTimeOffset.Now.ToLocalTime();
        user.UpdatedAt = null;
        user.Role = Role.USER;
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        return user;
    }
    
    public async Task<User> Put(UpdateUserDTO user, int id)
    {
        var findUser = await GetById(id);
        User userForUpdate = _mapper.Map(user, await GetById(id));

        var userEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == findUser.Email);
        var userPhone = await _dbContext.Users.FirstOrDefaultAsync(u => u.Phone == findUser.Phone);

        if (userEmail is not null && userPhone is not null)
        {
            if (userEmail.Id != id)
                throw new BadRequestException("You can't use this email", $"User with email: {userEmail.Email} already exist");

            if (userPhone.Id != id)
                throw new BadRequestException("You can't use this phone number",
                    $"User with this phone number: {userPhone.Phone} already exist");
        }

        userForUpdate.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
        
        _dbContext.Users.Update(userForUpdate);
        await _dbContext.SaveChangesAsync();
        
        return userForUpdate;
    }
    
    public async Task<int> DeleteById(int id)
    {
        _dbContext.Users.Remove(await GetById(id));
        await _dbContext.SaveChangesAsync();
        return id;
    }
}