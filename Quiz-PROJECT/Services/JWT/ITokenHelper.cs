using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services.JWT;

public interface ITokenHelper
{
    public string CreateToken(User user);
    public Task SetRefreshToken(RefreshTokenDTO newRefreshToken, CancellationToken token = default);
    public RefreshTokenDTO GenerateRefreshToken(long userId);
}