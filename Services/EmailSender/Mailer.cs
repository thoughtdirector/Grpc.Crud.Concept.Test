using Domain.Value_objects;
using MailKit.Security;
using MimeKit;
using Services.Services.Contract;
using System.Threading.Tasks;

namespace Services{
public class Mailer : IMailer
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public Mailer(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
    }

    public async Task SendEmailAsync(Message message)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress("Your Name", message.FromEmail));
        mimeMessage.To.Add(new MailboxAddress("", message.ToEmail));
        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart("plain")
        {
            Text = message.Body
        };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);

        // Optional: Authenticate with your email credentials
        await client.AuthenticateAsync(_smtpUsername, _smtpPassword);

        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}
}