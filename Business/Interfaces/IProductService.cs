using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IProductService
    {
        string GetTopPlatforms();
        List<ProductDTO> SearchProducts(string term, int limit, int offset);
        Task<ProductDTO> FindByIdAsync(int id);
        Task<ProductDTO> AddAsync(ProductDTO newProductDto);
        Task<ProductDTO> UpdateAsync(ProductDTO productDtoUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
