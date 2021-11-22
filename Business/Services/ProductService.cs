using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository, IMapper mapper, Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        public string GetTopPlatforms()
        {
            var pc = GetAllByPlatform(Platform.PersonalComputer);
            var mobile = GetAllByPlatform(Platform.Mobile);
            var ps = GetAllByPlatform(Platform.PlayStation);
            var xbox = GetAllByPlatform(Platform.Xbox);
            var nintendo = GetAllByPlatform(Platform.Nintendo);

            var platforms = new Dictionary<Platform, IEnumerable<string>>
            {
                {Platform.PersonalComputer, pc},
                {Platform.Mobile, mobile},
                {Platform.PlayStation, ps},
                {Platform.Xbox, xbox},
                {Platform.Nintendo, nintendo}
            };

            var platformsStr = new StringBuilder();
            foreach (var (platform, products) in platforms
                .OrderByDescending(p => p.Value.Count()).Take(3))
            {
                var productsStr = new StringBuilder();
                foreach (var product in products) productsStr.Append($"{product}\n");
                platformsStr.Append($"{platform} =\n{productsStr}\n");
            }

            return platformsStr.ToString();

            IEnumerable<string> GetAllByPlatform(Platform platform)
            {
                return _productRepository
                    .GetAll()
                    .Where(p => p.Platform.Equals(platform))
                    .Select(p => p.Name);
            }
        }

        public List<ProductOutputDTO> SearchProducts(string term, int? limit, int? offset, PageParameters pageParameters)
        {
            var productsList = new PagedList<Product>(
                _productRepository.GetAll(),
                     pageParameters.PageNumber,
                     pageParameters.PageSize);

            if (term.IsNullOrEmpty()) throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.NullValue);

            var productsCount = productsList.Count;
            limit ??= productsCount;
            offset ??= 0;

            if (limit < 0 || offset < 0) throw new HttpStatusException(
                HttpStatusCode.BadRequest, $"{ExceptionMessage.BadParameter}s {nameof(limit)} and {nameof(offset)} can't be less than 0");

            if (limit == 0 || offset > productsCount) return new List<ProductOutputDTO>();

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
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Id.Equals(id));
            if (product is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);
            return _mapper.Map<ProductOutputDTO>(product);
        }

        public async Task<ProductOutputDTO> AddAsync(ProductInputDTO newProductDto)
        {
            var newProduct = _mapper.Map<Product>(newProductDto);

            var downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(newProduct.Name + "_logo", newProductDto.Logo.OpenReadStream())
            });

            newProduct.Logo = downloadResult.Url.AbsolutePath;

            downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(newProduct.Name + "_background", newProductDto.Background.OpenReadStream())
            });

            newProduct.Background = downloadResult.Url.AbsolutePath;

            var product = await _productRepository.AddAsync(newProduct);
            if (product is null)
                throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.Fail);
            return _mapper.Map<ProductOutputDTO>(product);
        }

        public async Task<ProductOutputDTO> UpdateAsync(ProductInputDTO productDtoUpdate)
        {
            var oldProduct = await _productRepository
                .GetAll()
                .FirstOrDefaultAsync(p => p.Name.Equals(productDtoUpdate.Name));

            if (oldProduct is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            var newProduct = _mapper.Map(productDtoUpdate, oldProduct);

            var downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(newProduct.Name + "_logo", productDtoUpdate.Logo.OpenReadStream())
            });

            newProduct.Logo = downloadResult.Url.AbsolutePath;

            downloadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(newProduct.Name + "_background",
                    productDtoUpdate.Background.OpenReadStream())
            });

            newProduct.Background = downloadResult.Url.AbsolutePath;
            var result = await _productRepository.UpdateAsync(newProduct);
            return _mapper.Map<ProductOutputDTO>(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Id.Equals(id));
            if (product is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            await _productRepository.DeleteAsync(product);
        }

        public List<ProductOutputDTO> SearchProductsByFilters(
            PageParameters pageParameters, Genre genre, Rating rating, bool ratingAscending, bool priceAscending)
        {
            var products = _productRepository
                .GetAll()
                .Where(r => r.Rating >= rating);

            if (genre != Genre.All)
                products = products.Where(g => g.Genre.Equals(genre));

            products = ratingAscending 
            ? products.OrderBy(r => r.TotalRating) 
            : products.OrderByDescending(r => r.TotalRating);

            products = priceAscending 
                ? products.OrderBy(p => p.Price) 
                : products.OrderByDescending(p => p.Price);

            var productsList = new PagedList<Product>(
                products,
                pageParameters.PageNumber,
                pageParameters.PageSize);

            return productsList.Select(product => _mapper.Map<ProductOutputDTO>(product)).ToList();
        }
    }
}