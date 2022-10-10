namespace Quiz_PROJECT.Models.DTOModels;

public class RefreshTokenDTO
{
    public string Token { get; set; }
    public DateTimeOffset Expires { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long UserId { get; set; }
}