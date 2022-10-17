using System.Security.Cryptography;
using MailKit.Security;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;
using MimeKit.Text;
using Quiz_PROJECT.Models.DTOModels;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Quiz_PROJECT.Services;

public class MailKitService : IMailKitService
{
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;

    public MailKitService(IConfiguration configuration, IMemoryCache cache)
    {
        _cache = cache;
        _config = configuration.GetSection("AppSettings");
    }

    public async Task<string> SendMail(SendMailDTO request, CancellationToken token = default)
    {
        // dotnet user-secrets set "AppSettings:Email:Username" "...@gmail.com"
        // dotnet user-secrets set "AppSettings:Email:Password" "P@sSw0rd"

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["Email:Username"]));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

        using var smpt = new SmtpClient();
        await smpt.ConnectAsync(_config["Email:Host"], 587, SecureSocketOptions.StartTls, token);
        await smpt.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"], token);
        smpt.Timeout = 5000;

        await smpt.SendAsync(email, token);
        await smpt.DisconnectAsync(true, token);

        return $"Mail \n from: [{_config["Email:Username"]}] \n to: [{request.To}] \nhas been sent successfully";
    }

    public async Task<string> SendToConfirmEmail(string userEmail, CancellationToken token = default)
    {
        if (!_cache.TryGetValue(_config["MemoryCache:ConfirmEmailKey"], out string generatedConfirmEmailUrl))
        {
            _cache.Set(_config["MemoryCache:ConfirmEmailKey"],
                Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            return await SendToConfirmEmail(userEmail, token); // recursion update 'generatedConfirmEmailUrl' value
        }

        SendMailDTO sendToConfirm = new SendMailDTO
        {
            To = userEmail,
            Subject = "Confirm your email 😊 QuizApp",
            Body =
                $"To activate your account click <a href=\"https://localhost:7777/User/{userEmail}?confirmUrl={generatedConfirmEmailUrl}\">here</a>🚀"
        };

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["Email:Username"]));
        email.To.Add(MailboxAddress.Parse(sendToConfirm.To));
        email.Subject = sendToConfirm.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = sendToConfirm.Body };

        using var smpt = new SmtpClient();
        await smpt.ConnectAsync(_config["Email:Host"], 587, SecureSocketOptions.StartTls, token);
        await smpt.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"], token);
        smpt.Timeout = 5000;

        await smpt.SendAsync(email, token);
        await smpt.DisconnectAsync(true, token);

        return generatedConfirmEmailUrl;
    }
}