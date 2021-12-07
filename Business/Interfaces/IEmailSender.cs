using System.Threading.Tasks;
using DAL.Models.Entities;

namespace Business.Interfaces
{
    public interface IEmailSender
    {
        Task SendConfirmationEmailAsync(User user);
    }
}