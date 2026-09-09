using Microsoft.Extensions.Logging;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
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
        private readonly IProductVariantRepository _productVariantRepo;
        private readonly IImageConversionService _imageConversionService;
        private readonly IObjectStorage _objectStorage;

        public ProductService(IProductRepository productRepository,  ILogger<ProductService> logger, IProductVariantRepository productVariantRepository, IImageConversionService imageConversionService, IObjectStorage objectStorage)
        {
            _productRepo = productRepository;
            _productVariantRepo = productVariantRepository;
            _logger = logger;
            _imageConversionService = imageConversionService;
            _objectStorage = objectStorage;
        }

        public async Task<object> AddProductImageAsync(int productId, int productVariantId, Stream inputStream, AddProductImageRequest dto, CancellationToken cancellationToken)
        {
      
            await using var webpStream =
                await _imageConversionService.ConvertToWebPAsync(inputStream, cancellationToken);


            var imageId = Guid.NewGuid();

            var objectKey =
                $"products/{productId}/variants/{productVariantId}/{imageId}.webp";




            await _objectStorage.UploadAsync(
                objectKey,
                webpStream,
                "image/webp", 
                cancellationToken);



            await AddProductImageToProductVariantAsync(productVariantId, objectKey, dto, cancellationToken);

            return new
            {
                imageId,
                objectKey
            };
        }

        public async Task AddProductImageToProductVariantAsync(int productVariantId,string imageKey, AddProductImageRequest dto, CancellationToken cancellationToken)
        {
            
            var productImage = new ProductImage()
            {
                IsPrimary = dto.IsPrimary,
                StorageKey = imageKey,
                AltText = dto.AltText,
                SortOrder=dto.SortOrder,
                CreatedAt= DateTime.UtcNow,
            };

            await _productVariantRepo.AddProductImageToVariantAsync(productImage, productVariantId, cancellationToken);
        }

        public async Task<List<ProductCardDTO>> GetProductsAsync(string? search)
        {
            try
            {
                List<Product> products = await _productRepo.GetProductsWithSearch(search);

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
                        ImageUrl = _objectStorage.GetPublicUrl(cheapestVariant?.ProductImages.FirstOrDefault()?.StorageKey),
                        VariantId = cheapestVariant?.Id,
                        Keywords = p.ProductTags.Select(pt => pt.Tag.Name).ToList(),
                        Rating = new RatingDTO
                        {
                            AverageRating = p.AverageRating,
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

