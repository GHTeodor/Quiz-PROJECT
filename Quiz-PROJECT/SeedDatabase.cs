using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT;

public class SeedDatabase
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DBContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            byte[] passwordSalt;
            byte[] passwordHash;
            byte[] confirmPasswordHash;
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("string"));
                confirmPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("string"));
            };
            
            User user = new User()
            {
                FirstName = "IdentityAdmin",
                LastName = "IdentityUser",
                UserName = "Identity",
                Email = "Identity@gmail.com",
                Phone = "+38(012)34-56-789",
                
                PasswordHash = passwordHash,
                ConfirmPasswordHash = confirmPasswordHash,
                PasswordSalt = passwordSalt,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = Role.ADMIN,
                CreatedAt = DateTimeOffset.Now.ToLocalTime()
            };
            
            userManager.CreateAsync(user, "Password@123");
        }
    }
}