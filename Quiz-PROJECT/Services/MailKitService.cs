using System.Net.Mail;
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
        _config = configuration;
    }

    public async Task<string> SendMail(SendMailDTO request, CancellationToken token = default)
    {
        string mailFrom = _config.GetSection("EmailUsername").Value;

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(mailFrom));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

        using var smpt = new SmtpClient();
        await smpt.ConnectAsync(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls, token);
        await smpt.AuthenticateAsync(mailFrom, _config.GetSection("EmailPassword").Value, token);
        smpt.Timeout = 5000;
        
        await smpt.SendAsync(email, token);
        await smpt.DisconnectAsync(true, token);
        
        return $"Mail \n from: [{mailFrom}] \n to: [{request.To}] \nhas been sent successfully";
    }
}