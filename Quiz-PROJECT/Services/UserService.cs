using System.Text.RegularExpressions;
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

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _unitOfWork.Users.GetAllAsync());
    }
    
    public async Task<User> GetByIdAsync(long id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);;
    }

    public async Task<User> UpdateByIdAsync(UpdateUserDTO user, long id)
    {
        User userForUpdate = _mapper.Map(user, await GetByIdAsync(id));

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
        await _unitOfWork.DisposeAsync();

        return userForUpdate;
    }

    public async Task DeleteByIdAsync(long id)
    {
        await _unitOfWork.Users.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        await _unitOfWork.DisposeAsync();
    }
}