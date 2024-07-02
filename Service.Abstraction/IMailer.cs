
using Domain.Value_objects;
using System.Threading.Tasks;

namespace Services.Services.Contract
{
    public interface IMailer
    {
        Task SendEmailAsync(Message message);
    }

}