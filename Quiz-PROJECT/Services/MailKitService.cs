using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Quiz_PROJECT.Models.DTOModels;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Quiz_PROJECT.Services;

public class MailKitService : IMailKitService
{
    private readonly IConfiguration _config;

    public MailKitService(IConfiguration configuration)
    {
        _config = configuration.GetSection("AppSettings:Email");
    }

    public async Task<string> SendMail(SendMailDTO request, CancellationToken token = default)
    {
        // dotnet user-secrets set "AppSettings:Email:Username" "...@gmail.com"
        // dotnet user-secrets set "AppSettings:Email:Password" "P@sSw0rd"
        
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["Username"]));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

        using var smpt = new SmtpClient();
        await smpt.ConnectAsync(_config["Host"], 587, SecureSocketOptions.StartTls, token);
        await smpt.AuthenticateAsync(_config["Username"], _config["Password"], token);
        smpt.Timeout = 5000;
        
        await smpt.SendAsync(email, token);
        await smpt.DisconnectAsync(true, token);
        
        return $"Mail \n from: [{_config["Username"]}] \n to: [{request.To}] \nhas been sent successfully";
    }
}