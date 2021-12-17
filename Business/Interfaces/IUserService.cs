using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;
using Business.Parameters;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserInfoAsync(string userId);
        Task<UserDTO> UpdateAsync(string userId, UserDTO userDto);

        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword,
            string confirmationPassword);

        Task<IEnumerable<string>> GetUsersAsync(PageParameters pageParameters);
    }
}