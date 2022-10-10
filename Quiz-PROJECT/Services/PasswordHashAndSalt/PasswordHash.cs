using System.Security.Cryptography;

namespace Quiz_PROJECT.Services.PasswordHashAndSalt;

public class PasswordHash: IPasswordHash
{
    public void Create(string password, string confirmPassword, out byte[] passwordHash, out byte[] confirmPasswordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            confirmPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(confirmPassword));
        };
    }
    
    public bool Verify(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }
}