using System.Threading.Tasks;
using Business.DTO;
using DAL.Models.Entities;

namespace Business.Interfaces
{
    public interface ICloudinaryManager
    {
        Task<Product> UploadProductImagesAsync(Product product, ProductInputDTO productInputDto);
    }
}