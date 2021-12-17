using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using DAL.Interfaces;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public sealed class RatingManager : GenericRepository<ProductRating>, IRatingManager
    {
        public RatingManager(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ProductRating> FindByIdAsync(int productId, int userId) =>
            await DbContext.Set<ProductRating>().FindAsync(productId, userId);

        public async Task<IEnumerable<ProductRating>> GetByProductIdAsync(int productId) =>
            await DbContext.Set<ProductRating>()
                .Where(r => r.ProductId == productId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<bool> AnyAsync(int productId) =>
            await DbContext.Set<ProductRating>()
                .AnyAsync(r => r.ProductId == productId);

        public async Task<int> RecalculateRatingAsync(int productId)
        {
            var productRatings = DbContext.Set<ProductRating>()
                .Where(r => r.ProductId == productId)
                .AsNoTracking();

            var ratingsSum = await productRatings.SumAsync(r => r.Rating);
            var ratingsCount = await productRatings.CountAsync();

            return ratingsCount != 0
                ? ratingsSum / ratingsCount
                : 0;
        }
    }
}