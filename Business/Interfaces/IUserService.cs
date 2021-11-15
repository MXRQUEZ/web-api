using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> UpdateUserAsync(string userId, UserDTO userDto);
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword, string confirmationPassword);
        string GetUsers();
    }
}
