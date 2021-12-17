using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface ICloudinaryManager
    {
        Task<string> UploadProductLogoAsync(ProductInputDTO product);
        Task<string> UploadProductBackgroundAsync(ProductInputDTO product);
    }
}