using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IProductService
    {
        string GetTopPlatforms();
        List<ProductOutputDTO> SearchProducts(string term, int limit, int offset);
        Task<ProductOutputDTO> FindByIdAsync(int id);
        Task<ProductOutputDTO> AddAsync(ProductInputDTO newProductDto);
        Task<ProductOutputDTO> UpdateAsync(ProductInputDTO productDtoUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
