using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;

    public UserService(DBContext dbContext, IMapper mapper, IPasswordHash passwordHash, IMemoryCache cache, IConfiguration configuration)
    {
        _mapper = mapper;
        _passwordHash = passwordHash;
        _cache = cache;
        _config = configuration;
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

    public async Task<string> ConfirmEmailAsync(string email, string confirmUrl)
    {
        _cache.TryGetValue(_config["AppSettings:MemoryCache:ConfirmEmailKey"], out string confirmEmailString);

        if (confirmEmailString == confirmUrl.Replace(" ", "+"))
        {
            User user = await _unitOfWork.Users.FindByEmailAsync(email) 
                        ?? throw new NotFoundException("User not exist", $"There is no user with Eamil: {email}");
            if (user.EmailConfirmed == false)
            {
                user.EmailConfirmed = true;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.DisposeAsync();

                return "Email confirmed successfully";
            }

            return "Email has already been confirmed";
        }

        throw new BadRequestException("Email was not confirmed",$"Wrong confirmUrl: ({confirmUrl})");
    }
}