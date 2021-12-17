using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Business.Helpers
{
    public sealed class CloudinaryManager : ICloudinaryManager
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryManager(Cloudinary cloudinary) =>
            _cloudinary = cloudinary;

        public async Task<string> UploadProductLogoAsync(ProductInputDTO product)
        {
            var downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(product.Name + "_logo", product.Logo.OpenReadStream())
            });

            return downloadResult.Url.AbsolutePath;
        }

        public async Task<string> UploadProductBackgroundAsync(ProductInputDTO product)
        {
            var downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(product.Name + "_background", product.Background.OpenReadStream())
            });

            return downloadResult.Url.AbsolutePath;
        }
    }
}