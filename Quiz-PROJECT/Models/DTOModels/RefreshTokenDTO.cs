namespace Quiz_PROJECT.Models.DTOModels;

public class RefreshTokenDTO : BaseModelDTO
{
    public string Token { get; set; }
    public DateTimeOffset Expires { get; set; }
    public long UserId { get; set; }
}