using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<string> SignInAsync(UserCredentialsDTO userCredentialsDto);

        Task SignUpAsync(UserCredentialsDTO userCredentialsDto);

        Task ConfirmEmailAsync(string id, string token);
    }
}
