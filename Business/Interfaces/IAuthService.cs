using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<string> SignInAsync(UserCredentialsDTO userCredentialsDto);

        Task<bool> SignUpAsync(UserCredentialsDTO userCredentialsDto);

        Task<bool> ConfirmEmailAsync(string id, string token);
    }
}