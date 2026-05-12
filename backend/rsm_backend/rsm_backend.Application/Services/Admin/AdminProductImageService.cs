using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Admin
{
    public class AdminProductImageService : IAdminProductImageService
    {
        private readonly IProductImageRepository _productImageRepo;
        private readonly IProductVariantRepository _productVariantRepo;
        public AdminProductImageService(IProductImageRepository productImageRepository, IProductVariantRepository productVariantRepository) 
        {
            _productImageRepo = productImageRepository;
            _productVariantRepo= productVariantRepository;
        }
        public async Task<int> BulkCreateAsync(List<CreateProductImageDTO> productImageDTOs)
        {
            if (productImageDTOs == null) 
            {
                throw new ArgumentNullException(nameof(productImageDTOs));
            }

            if (productImageDTOs.Count == 0)
            {
                throw new ArgumentException("List of ProductImage DTO's is empty",nameof(productImageDTOs));
            }

            var duplicateInBatch = productImageDTOs
                                    .GroupBy(x => new { x.ProductVariantId, x.StorageKey })
                                    .FirstOrDefault(g => g.Count() > 1);

                                            if (duplicateInBatch != null)
                                            {
                                                throw new InvalidOperationException("Duplicate image found in request batch.");
                                            }

            DateTime now = DateTime.UtcNow;
            List<ProductImage> productImagesToAdd = new List<ProductImage>();

            var sortOrdersByVariant = new Dictionary<int, int>();

            foreach (var dto in productImageDTOs)
            {
                var productVariantExists = await _productVariantRepo.ExistsAsync(dto.ProductVariantId);

                if(!productVariantExists)
                {
                    throw new KeyNotFoundException($"ProductVariantId {dto.ProductVariantId} does not exist");
                }
                
                var productImageExists = await _productImageRepo.ExistsAsync(dto.ProductVariantId, dto.StorageKey);

                if(productImageExists)
                {
                    throw new InvalidOperationException($"ProductVariant with {dto.ProductVariantId} already has that imagekey");
                }

                if (!sortOrdersByVariant.ContainsKey(dto.ProductVariantId))
                {
                    var nextSortOrder = await _productImageRepo.GetNextSortOrderAsync(dto.ProductVariantId);
                    sortOrdersByVariant[dto.ProductVariantId] = nextSortOrder;
                }

                var sortOrder = sortOrdersByVariant[dto.ProductVariantId];

                productImagesToAdd.Add(new ProductImage()
                {
                    ProductVariantId = dto.ProductVariantId,
                    StorageKey = dto.StorageKey,
                    AltText = dto.AltText,
                    IsPrimary = dto.IsPrimary,
                    SortOrder = sortOrder,
                    CreatedAt =now,
                    UpdatedAt=now

                });

                sortOrdersByVariant[dto.ProductVariantId]++;
            }

            await _productImageRepo.BulkCreateAsync(productImagesToAdd);

            return productImagesToAdd.Count;
        }
    }
}
