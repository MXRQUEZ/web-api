using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using Business.Parameters;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductRating> _ratingRepository;

        public ProductService(IRepository<Product> productRepository, IRepository<ProductRating> ratingRepository, IMapper mapper, Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        public async Task<IEnumerable<Platform>> GetTopPlatformsAsync()
        {
            var pc = await GetAllByPlatformAsync(Platform.PersonalComputer);
            var mobile = await GetAllByPlatformAsync(Platform.Mobile);
            var ps = await GetAllByPlatformAsync(Platform.PlayStation);
            var xbox = await GetAllByPlatformAsync(Platform.Xbox);
            var nintendo = await GetAllByPlatformAsync(Platform.Nintendo);

            var platforms = new Dictionary<Platform, IEnumerable<string>>
            {
                {Platform.PersonalComputer, pc},
                {Platform.Mobile, mobile},
                {Platform.PlayStation, ps},
                {Platform.Xbox, xbox},
                {Platform.Nintendo, nintendo}
            };

            var topPlatforms = platforms
                .OrderByDescending(p => p.Value.Count())
                .Take(3)
                .Select(p => p.Key);

            return topPlatforms;

            async Task<IEnumerable<string>> GetAllByPlatformAsync(Platform platform)
            {
                return await _productRepository
                    .GetAll(false)
                    .OrderBy(on => on.Name)
                    .Where(p => p.Platform.Equals(platform))
                    .Select(p => p.Name)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<ProductOutputDTO>> SearchProductsAsync(
            string term, int? limit, int? offset, PageParameters pageParameters)
        {
            var products = await _productRepository
                .GetAll(false)
                .OrderBy(on => on.Name)
                .ToListAsync();
            var productsList =
                new PagedList<Product>(
                        products,
                        pageParameters.PageNumber,
                        pageParameters.PageSize);

            var productsCount = productsList.Count;
            limit ??= productsCount;
            offset ??= 0;

            if (limit < 0 || offset < 0)
                return null;

            if (limit == 0 || offset > productsCount) 
                return new List<ProductOutputDTO>();

            var termProducts = new List<ProductOutputDTO>();
            foreach (var product in productsList)
            {
                if (offset > 0)
                {
                    offset--;
                    continue;
                }

                if (limit <= 0) break;
                limit--;

                if (product.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase))
                    termProducts.Add(_mapper.Map<ProductOutputDTO>(product));
            }

            return termProducts;
        }

        public async Task<ProductOutputDTO> FindByIdAsync(int id)
        {
            var product = await _productRepository.GetAll(false).FirstOrDefaultAsync(p => p.Id.Equals(id));
            return product is null ? null : _mapper.Map<ProductOutputDTO>(product);
        }

        public async Task<ProductOutputDTO> AddAsync(ProductInputDTO newProductDto)
        {
            var newProduct = _mapper.Map<Product>(newProductDto);

            newProduct = await UploadImagesAsync(newProduct, newProductDto);
            await _productRepository.AddAndSaveAsync(newProduct);

            return _mapper.Map<ProductOutputDTO>(newProduct);
        }

        public async Task<ProductOutputDTO> UpdateAsync(ProductInputDTO productDtoUpdate)
        {
            var oldProduct = await _productRepository
                .GetAll(false)
                .FirstOrDefaultAsync(p => p.Name.Equals(productDtoUpdate.Name));

            if (oldProduct is null)
                return null;

            var newProduct = _mapper.Map(productDtoUpdate, oldProduct);

            newProduct = await UploadImagesAsync(newProduct, productDtoUpdate);
            await _productRepository.UpdateAndSaveAsync(newProduct);

            return _mapper.Map<ProductOutputDTO>(newProduct);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var product = await _productRepository.GetAll(false).FirstOrDefaultAsync(p => p.Id.Equals(id));
            if (product is null)
                return false;

            var ratings = _ratingRepository.GetAll(false);
            var rating = await ratings.FirstOrDefaultAsync(r => r.ProductId.Equals(id));
            if (rating is not null)
            {
                var deleteRatings = ratings.Where(r => r.ProductId.Equals(id));
                _ratingRepository.DeleteRange(deleteRatings);
            }

            await _productRepository.DeleteAndSaveAsync(product);
            return true;
        }

        public async Task<IEnumerable<ProductOutputDTO>> SearchProductsByFiltersAsync(
            PageParameters pageParameters, Genre genre, Rating rating, bool ratingAscending, bool priceAscending)
        {
            var products = await _productRepository.GetAll(false).OrderBy(on => on.Name).ToListAsync();
            var sortedProducts =
                products
                    .Where(r => r.Rating >= rating);

            if (genre != Genre.All)
                sortedProducts = sortedProducts.Where(g => g.Genre.Equals(genre));

            sortedProducts = ratingAscending 
            ? sortedProducts.OrderBy(r => r.TotalRating) 
            : sortedProducts.OrderByDescending(r => r.TotalRating);

            sortedProducts = priceAscending 
                ? sortedProducts.OrderBy(p => p.Price) 
                : sortedProducts.OrderByDescending(p => p.Price);

            var productsList = new PagedList<Product>(
                sortedProducts,
                pageParameters.PageNumber,
                pageParameters.PageSize);

            return productsList.Select(p => _mapper.Map<ProductOutputDTO>(p));
        }

        private async Task<Product> UploadImagesAsync(Product product, ProductInputDTO productInputDto)
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