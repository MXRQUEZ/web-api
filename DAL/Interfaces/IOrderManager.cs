using System.Threading.Tasks;
using DAL.Models.Entities;

namespace DAL.Interfaces
{
    public interface IOrderManager
    {
        Task<Order> FindByUserIdAsync(int userId);
        Task<Order> FindByIdAsync(int id);

        Task AddAndSaveAsync(Order entity);
        Task UpdateAndSaveAsync(Order entity);
        Task DeleteAndSaveAsync(Order entity);
    }
}