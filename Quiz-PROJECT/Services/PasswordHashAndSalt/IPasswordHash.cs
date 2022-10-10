namespace Quiz_PROJECT.Services.PasswordHashAndSalt;

public interface IPasswordHash
{
    void Create(string password, string confirmPassword, out byte[] passwordHash, out byte[] confirmPasswordHash,
        out byte[] passwordSalt);
    bool Verify(string password, byte[] passwordHash, byte[] passwordSalt);
}