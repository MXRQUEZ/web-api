using System.Threading.Tasks;
using DAL.Models;

namespace Business.Interfaces
{
    public interface IJwtGenerator
    {
        Task<string> GenerateTokenAsync(User user);
    }
}