using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services.JWT;

public class TokenHelper : ITokenHelper
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;

    public TokenHelper(IUnitOfWork unitOfWork, IMemoryCache cache, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _configuration = configuration;
    }

    public RefreshTokenDTO GenerateRefreshToken(long userId)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration["AppSettings:RefreshTokenSecretKey"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        var refreshToken = new RefreshTokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            CreatedAt = DateTimeOffset.UtcNow,
            UserId = userId
        };

        return refreshToken;
    }

    public async Task SetRefreshToken(RefreshTokenDTO newRefreshToken, CancellationToken token = default)
    {
        _cache.Set(_configuration["AppSettings:MemoryCache:RefreshToken"], newRefreshToken,
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(35)));

        var rT = await _unitOfWork.RefreshTokens.GetByUserIdAsync(newRefreshToken.UserId, token);
        if (rT is null)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = newRefreshToken.Token,
                CreatedAt = newRefreshToken.CreatedAt,
                UpdatedAt = null,
                Expires = newRefreshToken.Expires,
                UserId = newRefreshToken.UserId
            };
            await _unitOfWork.RefreshTokens.CreateAsync(refreshToken, token);
        }
        else
        {
            rT.Token = newRefreshToken.Token;
            rT.UpdatedAt = DateTimeOffset.UtcNow;
            rT.Expires = newRefreshToken.Expires;

            await _unitOfWork.RefreshTokens.UpdateAsync(rT);
        }

        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();
    }

    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration["AppSettings:AccessTokenSecretKey"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}