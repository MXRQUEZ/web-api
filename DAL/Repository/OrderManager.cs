using System.Linq;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using DAL.Interfaces;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public sealed class OrderManager : GenericRepository<Order>, IOrderManager
    {
        public OrderManager(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order> FindByIdAsync(int id) =>
            await DbContext.Set<Order>()
                .Include(order => order.OrderItems)
                .FirstOrDefaultAsync(order => order.Id == id);

        public async Task<Order> FindByUserIdAsync(int userId) =>
            await DbContext.Set<Order>()
                .Include(order => order.OrderItems)
                .FirstOrDefaultAsync(order => order.UserId == userId);

        public override IQueryable<Order> GetAll(bool trackChanges) =>
            trackChanges
                ? DbContext.Set<Order>().Include(order => order.OrderItems)
                : DbContext.Set<Order>().Include(order => order.OrderItems).AsNoTracking();
    }
}