using Domain.Models;
namespace ExternalServices.EmailSender
{
    public interface IMailer
    {
        Task SendEmailAsync(Message message);
    }

}
