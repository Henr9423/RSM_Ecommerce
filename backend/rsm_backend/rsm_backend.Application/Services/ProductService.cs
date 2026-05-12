using Microsoft.Extensions.Logging;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace rsm_backend.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository,  ILogger<ProductService> logger)
        {
            _productRepo = productRepository;
            _logger = logger;
        }
        public async Task<List<ProductCardDTO>> GetAllProducts()
        {
       
               try
            {
                List<Product> products = await _productRepo.GetAllProducts();

                return products.Select(p =>
                {
                    var cheapestVariant = p.ProductVariants
                        .OrderBy(v => v.Price)
                        .FirstOrDefault();

                    return new ProductCardDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = cheapestVariant?.Price,
                        ImageKey = cheapestVariant?.ProductImages.FirstOrDefault()?.StorageKey,
                        Keywords = p.ProductTags.Select(pt => pt.Tag.Name).ToList(),
                        Rating = new RatingDTO
                        {
                            Stars = p.AverageRating,
                            Count = p.RatingCount
                        }
                    };
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get products");
                throw;
            }


        }
    }
}

