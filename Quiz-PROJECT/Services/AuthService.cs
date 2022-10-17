using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services.JWT;
using Quiz_PROJECT.Services.PasswordHashAndSalt;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<User> _userManager;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;
    private readonly IPasswordHash _passwordHash;
    private readonly ITokenHelper _tokenHelper;

    public AuthService(DBContext dbContext, IMapper mapper,
        IHttpContextAccessor contextAccessor, UserManager<User> userManager, IMemoryCache memoryCache,
        IPasswordHash passwordHash, ITokenHelper tokenHelper, IConfiguration configuration)
    {
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _cache = memoryCache;
        _passwordHash = passwordHash;
        _tokenHelper = tokenHelper;
        _config = configuration;
        _unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken token = default)
    {
        return await _userManager.Users.ToListAsync(token);
    }

    public async Task<Dictionary<string, string>> GetInfoFromTokenAsync()
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        if (_contextAccessor.HttpContext is not null)
        {
            result.Add("id", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            result.Add("userName", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name));
            result.Add("email", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            result.Add("mobilePhone", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone));
            result.Add("role", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role));
        }

        return await Task.FromResult(result);
    }

    public async Task<User> RegistrationAsync(CreateUserDTO request, CancellationToken token = default)
    {
        User user = _mapper.Map<User>(request);

        _passwordHash.Create(request.Password, request.ConfirmPassword, out byte[] passwordHash,
            out byte[] confirmPasswordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.ConfirmPasswordHash = confirmPasswordHash;
        user.PasswordSalt = passwordSalt;

        user.UpdatedAt = null;
        user.Role = Role.USER;

        await _unitOfWork.Users.CreateAsync(user, token);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();

        _cache.Set(_config["AppSettings:MemoryCache:ConfirmEmailKey"],
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

        return user;
    }

    public async Task<Login_Refresh_JWTResponseDTO> LoginAsync(AuthLoginUserDTO request,
        CancellationToken token = default)
    {
        User user = await _unitOfWork.Users.FindByEmailAsync(request.Email, token);

        if (user is null)
            throw new BadRequestException("User not found", $"No user with this email {user.Email}");

        if (!_passwordHash.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new BadRequestException("Wrong email or password",
                $"No user with this email: {user.Email} or password");

        var refreshToken = _tokenHelper.GenerateRefreshToken(user.Id);
        await _tokenHelper.SetRefreshToken(refreshToken, token);

        return new Login_Refresh_JWTResponseDTO { Id = user.Id, AccessToken = _tokenHelper.CreateToken(user) };
    }

    public async Task<Login_Refresh_JWTResponseDTO> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        _cache.TryGetValue(_config["AppSettings:MemoryCache:RefreshToken"], out RefreshTokenDTO refreshToken);

        User user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId, cancellationToken);

        if (!user.RefreshToken.Token.Equals(refreshToken.Token))
            throw new UnauthorizedException("Invalid Refresh Token", "re-login");

        if (user.RefreshToken.Expires <= DateTimeOffset.UtcNow)
            throw new UnauthorizedException("Token expired", "re-login");

        string token = _tokenHelper.CreateToken(user);
        var newRefreshToken = _tokenHelper.GenerateRefreshToken(user.Id);
        await _tokenHelper.SetRefreshToken(newRefreshToken, cancellationToken);

        return new Login_Refresh_JWTResponseDTO { Id = user.Id, AccessToken = token };
    }

    public async Task LogoutAsync(CancellationToken token = default)
    {
        _cache.TryGetValue(_config["AppSettings:MemoryCache:RefreshToken"], out RefreshTokenDTO refreshToken);

        await _unitOfWork.RefreshTokens.DeleteByUserIdAsync(refreshToken.UserId, token);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();

        // _cache.Dispose();
    }
}