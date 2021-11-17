﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public string GetTopPlatforms()
        {
            var platforms = _productRepository.GetTopPlatforms();
            var platformsStr = new StringBuilder();
            foreach (var (platform, products) in platforms)
            {
                var productsStr = new StringBuilder();
                foreach (var product in products)
                {
                    productsStr.Append($"{product}\n");
                }
                platformsStr.Append($"{platform} =\n{productsStr}\n");
            }

            return platformsStr.ToString();
        }

        public List<ProductDTO> SearchProducts(string term, int limit, int offset)
        {
            if (term is null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            if (limit < 0 || offset < 0)
            {
                throw new ArgumentException($"{nameof(limit)} or {nameof(offset)}");
            }

            if (limit == 0 || offset > _productRepository.GetProducts().Count())
            {
                return new List<ProductDTO>();
            }

            var termProducts = new List<ProductDTO>();
            foreach (var product in _productRepository.GetProducts())
            {
                if (offset > 0)
                {
                    offset--;
                    continue;
                }

                if (limit <= 0) break;
                limit--;

                if (product.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase))
                {
                    
                    termProducts.Add(_mapper.Map<ProductDTO>(product));
                }
            }

            return termProducts;
        }

        public async Task<ProductDTO> FindByIdAsync(int id)
        {
            var product = await _productRepository.FindByIdAsync(id);
            if (product is null) throw new ArgumentException("There is no product with this id");
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> AddAsync(ProductDTO newProductDto)
        {
            var newProduct = _mapper.Map<Product>(newProductDto);
            var product = await _productRepository.AddAsync(newProduct);
            if (product is null)
                throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.Fail);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO productDtoUpdate)
        {
            var oldProduct = _productRepository.GetProducts().FirstOrDefault(p => p.Name == productDtoUpdate.Name);
            if (oldProduct is null) throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.NotFound);
            var newProduct = _mapper.Map(productDtoUpdate, oldProduct);
            var result = await _productRepository.UpdateAsync(newProduct);
            return _mapper.Map<ProductDTO>(result);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var isUserFound = await _productRepository.DeleteByIdAsync(id);
            if (!isUserFound) throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.NotFound);
            return true;
        }
    }
}
