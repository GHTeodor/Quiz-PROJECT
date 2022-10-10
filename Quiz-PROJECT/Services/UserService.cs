using AutoMapper;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services.PasswordHashAndSalt;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHash _passwordHash;

    public UserService(DBContext dbContext, IMapper mapper, IPasswordHash passwordHash)
    {
        _mapper = mapper;
        _passwordHash = passwordHash;
        _unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken token = default)
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _unitOfWork.Users.GetAllAsync(token));
    }
    
    public async Task<User> GetByIdAsync(long id, CancellationToken token = default)
    {
        return await _unitOfWork.Users.GetByIdAsync(id, token);
    }
    
    public async Task<User> UpdateByIdAsync(UpdateUserDTO user, long id, CancellationToken token = default)
    {
        User updatedUser = _mapper.Map(user, await _unitOfWork.Users.GetByIdAsync(id, token));
        
        _passwordHash.Create(user.Password, user.ConfirmPassword, out byte[] passwordHash, out byte[] confirmPasswordHash, out byte[] passwordSalt);
        updatedUser.PasswordHash = passwordHash;
        updatedUser.ConfirmPasswordHash = confirmPasswordHash;
        updatedUser.PasswordSalt = passwordSalt;
        
        // Check if user will have unique email and phone number after update
        var userByEmail = await _unitOfWork.Users.FindByEmailAsync(updatedUser.Email, token);
        var userByPhone = await _unitOfWork.Users.FindByPhoneAsync(updatedUser.Phone, token);
        
        if (userByEmail is not null && userByEmail.Id != id)
            throw new BadRequestException("You can't use this email",
                $"User with email: {updatedUser.Email} already exist");
        
        if (userByPhone is not null && userByPhone.Id != id)
            throw new BadRequestException("You can't use this phone number",
                $"User with this phone number: {updatedUser.Phone} already exist");
        //
        
        updatedUser.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
        
        await _unitOfWork.Users.UpdateAsync(updatedUser);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();
        
        return updatedUser;
    }

    public async Task DeleteByIdAsync(long id, CancellationToken token = default)
    {
        await _unitOfWork.Users.DeleteByIdAsync(id, token);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();
    }
}