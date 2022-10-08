using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Quiz_PROJECT.Errors;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<User> _userManager;

    public AuthService(IConfiguration configuration, DBContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
    {
        _configuration = configuration;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken token = default)
    {
        return await _userManager.Users.ToListAsync(token);
    }

    public async Task<Dictionary<string, string>> GetInfoFromTokenAsync(CancellationToken token = default)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        
        if (_contextAccessor.HttpContext is not null)
        {
            result.Add("UserName", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name));
            result.Add("Email", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            result.Add("MobilePhone", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone));
            result.Add("Role", _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role));
        }
        return await Task.FromResult(result);
    }

    public async Task<User> RegistrationAsync(CreateUserDTO request, CancellationToken token = default)
    {
        User user = _mapper.Map<User>(request);

        _CreatePasswordHash(request.Password, request.ConfirmPassword, out byte[] passwordHash, out byte[] confirmPasswordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.ConfirmPasswordHash = confirmPasswordHash;
        user.PasswordSalt = passwordSalt;
        
        user.UpdatedAt = null;
        user.Role = Role.USER;
        
        await _unitOfWork.Users.CreateAsync(user, token);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();

        return user;
    }

    public async Task<string> LoginAsync(AuthLoginUserDTO request, CancellationToken token = default)
    {
        User user = await _unitOfWork.Users.FindByEmailAsync(request.Email, token);
        
        if (user is null)
            throw new BadRequestException("User not found", $"No user with this email {user.Email}");

        if (!_VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new BadRequestException("Wrong email or password", $"No user with this email: {user.Email} or password");

        var refreshToken = _GenerateRefreshToken();
        await _SetRefreshToken(refreshToken, user.Id, token);

        return JsonConvert.SerializeObject(_CreateToken(user));
    }

    public async Task<string> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var refreshToken = _contextAccessor.HttpContext.Request.Cookies["refreshToken"];
        long userId = long.Parse(_contextAccessor.HttpContext.Request.Cookies["userId"]!);
        
        User user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
    
        if (!user.RefreshToken.Token.Equals(refreshToken))
            throw new UnauthorizedException("Invalid Refresh Token", "re-login");
        
        if (user.RefreshToken.Expires <= DateTimeOffset.Now.ToLocalTime())
            throw new UnauthorizedException("Token expired", "re-login");
    
        string token = _CreateToken(user);
        var newRefreshToken = _GenerateRefreshToken();
        await _SetRefreshToken(newRefreshToken, user.Id, cancellationToken);
        
        return await Task.FromResult(token);
    }

    public async Task<User> UpdateByIdAsync(UpdateUserDTO user, long id, CancellationToken token = default)
    {
        User updatedUser = _mapper.Map(user, await _unitOfWork.Users.GetByIdAsync(id));
        
        _CreatePasswordHash(user.Password, user.ConfirmPassword, out byte[] passwordHash, out byte[] confirmPasswordHash, out byte[] passwordSalt);
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

    public async Task LogoutAsync(CancellationToken token = default)
    {
        long userId = long.Parse(_contextAccessor.HttpContext.Request.Cookies["userId"]!);

        await _unitOfWork.RefreshTokens.DeleteByUserIdAsync(userId, token);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();
        
        _contextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
        _contextAccessor.HttpContext.Response.Cookies.Delete("userId");
    }

    // ---------------------------------------------------------------------

    private _RefreshToken _GenerateRefreshToken()
    {
        var refreshToken = new _RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTimeOffset.Now.AddDays(7),
        };

        return refreshToken;
    }

    private async Task _SetRefreshToken(_RefreshToken newRefreshToken, long userId, CancellationToken token = default)
    {
        CookieOptions cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        
        _contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        _contextAccessor.HttpContext.Response.Cookies.Append("userId", userId.ToString(), cookieOptions);
        
        var rT = await _unitOfWork.RefreshTokens.GetByUserIdAsync(userId, token);
        if (rT is null)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = newRefreshToken.Token,
                CreatedAt = newRefreshToken.CreatedAt,
                UpdatedAt = null,
                Expires = newRefreshToken.Expires,
                UserId = userId
            };
            await _unitOfWork.RefreshTokens.CreateAsync(refreshToken, token);
        }
        else
        {
            rT.Token = newRefreshToken.Token;
            rT.UpdatedAt = DateTimeOffset.Now.ToLocalTime();
            rT.Expires = newRefreshToken.Expires;
            
            await _unitOfWork.RefreshTokens.UpdateAsync(rT);
        }

        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();
    }
    
    private string _CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private void _CreatePasswordHash(string password, string confirmPassword, out byte[] passwordHash, out byte[] confirmPasswordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            confirmPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(confirmPassword));
        };
    }

    private bool _VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }
    private class _RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt => DateTimeOffset.Now.ToLocalTime();
        public DateTimeOffset Expires { get; set; }
    }
}