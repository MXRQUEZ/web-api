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
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        private readonly IProductRepository<Product> _productRepository;
        private readonly UserManager<User> _userManager;

        public ProductService(IProductRepository<Product> productRepository, IMapper mapper, Cloudinary cloudinary,
            UserManager<User> userManager)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _userManager = userManager;
        }

        public string GetTopPlatforms()
        {
            var pc = _productRepository.GetProductsByPlatform(Platform.PersonalComputer);
            var mobile = _productRepository.GetProductsByPlatform(Platform.Mobile);
            var ps = _productRepository.GetProductsByPlatform(Platform.PlayStation);
            var xbox = _productRepository.GetProductsByPlatform(Platform.Xbox);
            var nintendo = _productRepository.GetProductsByPlatform(Platform.Nintendo);

            var platforms = new Dictionary<Platform, IEnumerable<string>>
            {
                {Platform.PersonalComputer, pc},
                {Platform.Mobile, mobile},
                {Platform.PlayStation, ps},
                {Platform.Xbox, xbox},
                {Platform.Nintendo, nintendo}
            };

            var platformsStr = new StringBuilder();
            foreach (var (platform, products) in platforms.OrderByDescending(p => p.Value.Count()).Take(3))
            {
                var productsStr = new StringBuilder();
                foreach (var product in products) productsStr.Append($"{product}\n");
                platformsStr.Append($"{platform} =\n{productsStr}\n");
            }

            return platformsStr.ToString();
        }

        public List<ProductOutputDTO> SearchProducts(string term, int? limit, int? offset, PageParameters productParameters)
        {
            var productsList = PagedList<Product>.ToPagedList(
                _productRepository.GetAllProducts(),
                     productParameters.PageNumber,
                     productParameters.PageSize);

            if (term is null) throw new ArgumentNullException(nameof(term));

            var productsCount = productsList.Count;
            limit ??= productsCount;
            offset ??= 0;

            if (limit < 0 || offset < 0) throw new ArgumentException($"{nameof(limit)} or {nameof(offset)}");

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
            var product = await _productRepository.FindByIdAsync(id);
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
            var oldProduct = GetProduct(productDtoUpdate.Name);

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
            var isProductFound = await _productRepository.DeleteByIdAsync(id);
            if (!isProductFound)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);
        }

        public async Task<ProductOutputDTO> RateAsync(string userId, int rating, string productName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var product = GetProduct(productName, rating);

            if (product is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            var userRating = product.Ratings.FirstOrDefault(r => r.User == user);
            if (userRating is null)
                product.Ratings.Add(new ProductRating {Product = product, User = user, Rating = rating});
            else
                userRating.Rating = rating;

            product.TotalRating = product.Ratings.Sum(r => r.Rating) / product.Ratings.Count;
            var result = await _productRepository.UpdateAsync(product);

            return _mapper.Map<ProductOutputDTO>(result);
        }

        public async Task DeleteRatingAsync(string userId, string productName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var product = GetProduct(productName);

            var userRating = product.Ratings.FirstOrDefault(r => r.User == user);
            if (userRating is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            product.Ratings.Remove(userRating);

            var ratingsCount = product.Ratings.Count;
            if (ratingsCount == 0)
                product.TotalRating = 0;
            else
                product.TotalRating = product.Ratings.Sum(r => r.Rating) / ratingsCount;

            await _productRepository.UpdateAsync(product);
        }

        private Product GetProduct(string productName, int rating = 0)
        {
            if (rating is > 100 or < 0)
                throw new ArgumentException("Rating can't be more than 100 or less than 0");

            if (productName is null)
                throw new ArgumentNullException(nameof(productName), "can't be null");

            var product = _productRepository
                .GetAllProducts()
                .FirstOrDefault(p => p.Name == productName);

            if (product is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            return product;
        }

        public List<ProductOutputDTO> SearchProductsByFilters(
            PageParameters pageParameters, Genre genre, Rating rating, bool ratingAscending, bool priceAscending)
        {
            var products = _productRepository.GetAllProducts().Where(r => r.Rating >= rating);

            if (genre != Genre.All)
                products = products.Where(g => g.Genre == genre);

            products = ratingAscending 
            ? products.OrderBy(r => r.TotalRating) 
            : products.OrderByDescending(r => r.TotalRating);

            products = priceAscending 
                ? products.OrderBy(p => p.Price) 
                : products.OrderByDescending(p => p.Price);

            var productsList = PagedList<Product>.ToPagedList(
                products,
                pageParameters.PageNumber,
                pageParameters.PageSize);

            return productsList.Select(product => _mapper.Map<ProductOutputDTO>(product)).ToList();
        }
    }
}