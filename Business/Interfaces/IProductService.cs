using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;
using Business.Parameters;
using DAL.Models;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Platform>> GetTopPlatformsAsync();
        Task<IEnumerable<ProductOutputDTO>> SearchProductsAsync(
            string term, int? limit, int? offset, PageParameters pageParameters);
        Task<IEnumerable<ProductOutputDTO>> SearchProductsByFiltersAsync(
            PageParameters pageParameters, ProductFilters productFilters);
        Task<ProductOutputDTO> FindByIdAsync(int id);
        Task<ProductOutputDTO> AddAsync(ProductInputDTO newProductDto);
        Task<ProductOutputDTO> UpdateAsync(ProductInputDTO productDtoUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
