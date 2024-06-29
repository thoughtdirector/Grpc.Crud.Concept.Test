using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalServices.EmailSender
{
    public interface IEmailService
    {
        Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body);
    }
}
