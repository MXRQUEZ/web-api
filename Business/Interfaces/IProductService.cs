using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;
using Business.Parameters;
using DAL.Models;

namespace Business.Interfaces
{
    public interface IProductService
    {
        string GetTopPlatforms();
        List<ProductOutputDTO> SearchProducts(string term, int? limit, int? offset, PageParameters pageParameters);
        public List<ProductOutputDTO> SearchProductsByFilters(PageParameters pageParameters,
            Genre genre, Rating rating, bool ratingAscending, bool priceAscending);
        Task<ProductOutputDTO> FindByIdAsync(int id);
        Task<ProductOutputDTO> AddAsync(ProductInputDTO newProductDto);
        Task<ProductOutputDTO> UpdateAsync(ProductInputDTO productDtoUpdate);
        Task DeleteByIdAsync(int id);
        Task DeleteRatingAsync(string userId, string productName);
        Task<ProductOutputDTO> RateAsync(string userId, int rating, string productName);
    }
}
