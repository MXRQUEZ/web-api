using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> SignInAsync(UserCredentialsDTO userCredentialsDto);

        Task<bool> SignUpAsync(UserCredentialsDTO userCredentialsDto);

        Task<bool> ConfirmEmailAsync(int id, string token);
    }
}
