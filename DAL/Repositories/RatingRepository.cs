using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using DAL.Interfaces;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public sealed class RatingRepository : IRepository<ProductRating>, IDisposable
    {
        private readonly ApplicationDbContext _db;

        public RatingRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public IQueryable<ProductRating> GetAll()
        {
            return _db.Set<ProductRating>()
                .OrderBy(on => on.ProductId)
                .AsNoTracking();
        }

        public async Task<ProductRating> AddAsync(ProductRating newRating)
        {
            await _db.Set<ProductRating>().AddAsync(newRating);
            await _db.SaveChangesAsync();

            return newRating;
        }

        public async Task<ProductRating> UpdateAsync(ProductRating ratingUpdate)
        {
            _db.Set<ProductRating>().Update(ratingUpdate);
            await _db.SaveChangesAsync();

            return ratingUpdate;
        }

        public async Task DeleteAsync(ProductRating rating)
        {
            _db.Set<ProductRating>().Remove(rating);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
