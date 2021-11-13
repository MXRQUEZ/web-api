using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using IMapper = AutoMapper.IMapper;

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
                throw new ArgumentException($"{nameof(limit)} and {nameof(offset)} can't be less than 0");
            }

            if (limit == 0)
            {
                return new List<ProductDTO>();
            }

            var termProducts = new List<ProductDTO>();
            term = term.ToLower();
            foreach (var product in _productRepository.GetProducts())
            {
                var lowerCaseProductName = product.Name.ToLower();
                if (limit <= 0) break;
                limit--;

                if (offset >= 0)
                {
                    offset--;
                    continue;
                }

                if (lowerCaseProductName.Contains(term))
                {
                    
                    termProducts.Add(_mapper.Map<ProductDTO>(product));
                }
            }

            return termProducts;
        }
    }
}
