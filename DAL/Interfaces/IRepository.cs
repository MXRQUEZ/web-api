using System.Collections.Generic;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Dictionary<Platforms, string[]> GetTopPlatforms();
        public IEnumerable<Product> GetProducts();
    }
}
