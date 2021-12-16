using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAL.Models.Entities;

namespace Business.Helpers
{
    public sealed class CloudinaryManager : ICloudinaryManager
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryManager(Cloudinary cloudinary) => _cloudinary = cloudinary;

        public async Task<Product> UploadProductImagesAsync(Product product, ProductInputDTO productInputDto)
        {
            var downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(product.Name + "_logo", productInputDto.Logo.OpenReadStream())
            });

            product.Logo = downloadResult.Url.AbsolutePath;

            downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(product.Name + "_background", productInputDto.Background.OpenReadStream())
            });

            product.Background = downloadResult.Url.AbsolutePath;

            return product;
        }
    }
}
