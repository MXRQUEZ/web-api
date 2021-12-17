using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Business.Parameters;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Entities;

namespace Business.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly ICloudinaryManager _cloudinaryManager;
        private readonly IMapper _mapper;
        private readonly IProductManager _productManager;
        private readonly IRatingManager _ratingManager;

        public ProductService(
            IProductManager productManager, IRatingManager ratingManager, IMapper mapper,
            ICloudinaryManager cloudinaryManager)
        {
            _productManager = productManager;
            _ratingManager = ratingManager;
            _cloudinaryManager = cloudinaryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Platform>> GetTopPlatformsAsync()
        {
            var pcCount = await _productManager.CountByPlatformAsync(Platform.PersonalComputer);
            var mobileCount = await _productManager.CountByPlatformAsync(Platform.Mobile);
            var psCount = await _productManager.CountByPlatformAsync(Platform.PlayStation);
            var xboxCount = await _productManager.CountByPlatformAsync(Platform.Xbox);
            var nintendoCount = await _productManager.CountByPlatformAsync(Platform.Nintendo);

            var platforms = new Dictionary<Platform, int>
            {
                {Platform.PersonalComputer, pcCount},
                {Platform.Mobile, mobileCount},
                {Platform.PlayStation, psCount},
                {Platform.Xbox, xboxCount},
                {Platform.Nintendo, nintendoCount}
            };

            var topPlatforms = platforms
                .OrderByDescending(p => p.Value)
                .Take(3)
                .Select(p => p.Key);

            return topPlatforms;
        }

        public async Task<IEnumerable<ProductOutputDTO>> SearchByTermAsync(
            string term, int? limit, int? offset, PageParameters pageParameters)
        {
            var products = await _productManager
                .GetByTermAsync(term);
            var productsList =
                new PagedList<Product>(
                    products,
                    pageParameters.PageNumber,
                    pageParameters.PageSize);

            var productsCount = productsList.Count;
            limit ??= productsCount;
            offset ??= 0;

            if (limit < 0 || offset < 0)
                return await Task.FromResult<IEnumerable<ProductOutputDTO>>(null);

            if (limit == 0 || offset > productsCount)
                return new List<ProductOutputDTO>();

            var termProducts = new List<ProductOutputDTO>();
            foreach (var product in productsList.Skip(offset.Value))
            {
                if (limit <= 0) break;
                limit--;

                termProducts.Add(_mapper.Map<ProductOutputDTO>(product));
            }

            return termProducts;
        }

        public async Task<ProductOutputDTO> FindByIdAsync(int id)
        {
            var product = await _productManager.FindByIdAsync(id);
            return product is null
                ? await Task.FromResult<ProductOutputDTO>(null)
                : _mapper.Map<ProductOutputDTO>(product);
        }

        public async Task<ProductOutputDTO> AddAsync(ProductInputDTO newProductDto)
        {
            var newProduct = _mapper.Map<Product>(newProductDto);

            newProduct.Logo = await _cloudinaryManager.UploadProductLogoAsync(newProductDto);
            newProduct.Background = await _cloudinaryManager.UploadProductBackgroundAsync(newProductDto);
            await _productManager.AddAndSaveAsync(newProduct);

            return _mapper.Map<ProductOutputDTO>(newProduct);
        }

        public async Task<ProductOutputDTO> UpdateAsync(ProductInputDTO productDtoUpdate)
        {
            var oldProduct = await _productManager
                .FindByNameAsync(productDtoUpdate.Name);

            if (oldProduct is null)
                return await Task.FromResult<ProductOutputDTO>(null);

            var newProduct = _mapper.Map(productDtoUpdate, oldProduct);

            newProduct.Logo = await _cloudinaryManager.UploadProductLogoAsync(productDtoUpdate);
            newProduct.Background = await _cloudinaryManager.UploadProductBackgroundAsync(productDtoUpdate);
            await _productManager.UpdateAndSaveAsync(newProduct);

            return _mapper.Map<ProductOutputDTO>(newProduct);
        }

        public async Task<bool> DeleteByIdAsync(int productId)
        {
            var product = await _productManager.FindByIdAsync(productId);
            if (product is null)
                return false;

            if (!await _ratingManager.AnyAsync(productId))
            {
                await _productManager.DeleteAndSaveAsync(product);
                return true;
            }

            var deleteRatings = await _ratingManager.GetByProductIdAsync(productId);
            _ratingManager.DeleteRange(deleteRatings);
            await _productManager.DeleteAndSaveAsync(product);
            return true;
        }

        public async Task<IEnumerable<ProductOutputDTO>> SearchByFiltersAsync(
            PageParameters pageParameters, ProductFilters productFilters)
        {
            var sortedProducts =
                await _productManager.GetWhereAsync(product => product.Rating >= productFilters.Rating);

            if (productFilters.Genre != Genre.All)
                sortedProducts = sortedProducts.Where(product => product.Genre == productFilters.Genre);

            sortedProducts = productFilters.RatingAscending
                ? sortedProducts.OrderBy(product => product.TotalRating)
                : sortedProducts.OrderByDescending(product => product.TotalRating);

            sortedProducts = productFilters.PriceAscending
                ? sortedProducts.OrderBy(product => product.Price)
                : sortedProducts.OrderByDescending(product => product.Price);

            var productsList = new PagedList<Product>(
                sortedProducts,
                pageParameters.PageNumber,
                pageParameters.PageSize);

            return productsList.Select(product => _mapper.Map<ProductOutputDTO>(product));
        }
    }
}