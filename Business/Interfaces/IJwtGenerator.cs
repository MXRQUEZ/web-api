using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Entities;

namespace Business.Interfaces
{
    public interface IJwtGenerator
    {
        Task<string> GenerateTokenAsync(User user);
    }
}