using Domain.Value_objects;
using Services.Services.Contract;
using System.Threading.Tasks;

namespace Services { 
public class EmailService : IEmailService
{
    private readonly IMailer _mailer;

    public EmailService(IMailer mailer)
    {
        _mailer = mailer;
    }

    public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body)
    {
        var message = new Message
        {
            FromEmail = fromEmail,
            ToEmail = toEmail,
            Subject = subject,
            Body = body
        };

        await _mailer.SendEmailAsync(message);
           
    }
}
}