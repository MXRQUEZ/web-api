using System.Threading.Tasks;
using Business.DTO;
using Business.Parameters;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> UpdateAsync(string userId, UserDTO userDto);
        Task ChangePasswordAsync(string userId, string oldPassword, string newPassword, string confirmationPassword);
        string GetUsers(PageParameters pageParameters);
    }
}
