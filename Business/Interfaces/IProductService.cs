using System.Collections.Generic;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IProductService
    {
        string GetTopPlatforms();
        List<ProductDTO> SearchProducts(string term, int limit, int offset);
    }
}
