using AutoMapper;
using Newtonsoft.Json;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(DBContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
    }

    public async Task<IEnumerable<UserDTO>> Get()
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _unitOfWork.Users.GetAllAsync());
    }
    
    public async Task<User> GetById(int id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);;
    }
    
    public async Task<User> Post(CreateUserDTO createdUser)
    {
        if (createdUser == null)
        {
            throw new BadRequestException("Wrong field(s)",
                $"User's fields: {JsonConvert.SerializeObject(typeof(User).GetProperties().Select(f => f.Name))}");
        }

        User user = _mapper.Map<User>(createdUser);
        
        user.CreatedAt = DateTimeOffset.Now.ToLocalTime();
        user.UpdatedAt = null;
        user.Role = Role.USER;
        
        await _unitOfWork.Users.CreateAsync(user);
        await _unitOfWork.SaveAsync();
        
        return user;
    }
    
    public async Task<User> Put(UpdateUserDTO user, int id)
    {
        User userForUpdate = _mapper.Map(user, await GetById(id));

        // Check if user will have unique email and phone number after update
        var userByEmail = await _unitOfWork.Users.FindByEmailAsync(userForUpdate.Email);
        var userByPhone = await _unitOfWork.Users.FindByPhoneAsync(userForUpdate.Phone);
        
        if (userByEmail is not null && userByEmail.Id != id)
            throw new BadRequestException("You can't use this email",
                $"User with email: {userForUpdate.Email} already exist");

        if (userByPhone is not null && userByPhone.Id != id)
            throw new BadRequestException("You can't use this phone number",
                $"User with this phone number: {userForUpdate.Phone} already exist");
        //

        userForUpdate.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
        
        await _unitOfWork.Users.UpdateAsync(userForUpdate);
        await _unitOfWork.SaveAsync();

        return userForUpdate;
    }

    public async Task DeleteById(int id)
    {
        await _unitOfWork.Users.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }
}