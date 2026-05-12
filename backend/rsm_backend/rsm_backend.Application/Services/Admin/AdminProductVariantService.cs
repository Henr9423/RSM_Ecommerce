using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Admin
{
    public class AdminProductVariantService : IAdminProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepo;
        private readonly IProductRepository _productRepo;

        public AdminProductVariantService (IProductVariantRepository productVariantRepo, IProductRepository productRepository)
        {
            _productVariantRepo = productVariantRepo;
            _productRepo = productRepository;
        }

        public async Task<int> BulkCreateAsync(List<CreateProductVariantDTO> productVariantDTOs)
        {
            if(productVariantDTOs==null)
            {
                throw new NullReferenceException();
            }

            if(productVariantDTOs.Count==0)
            {
                throw new ArgumentException("List of productVariant DTO's can't be empty", nameof(productVariantDTOs));
            }
            
            var duplicateExists = productVariantDTOs
             .GroupBy(pv => new { pv.ProductId, pv.Sku})
             .Any(g => g.Count() > 1);

            if (duplicateExists)
                throw new ArgumentException("Request contains duplicate product/tag combinations.", nameof(productVariantDTOs));

            var now = DateTime.UtcNow;


            var productVariantsToAdd = new List<ProductVariant>();

            foreach (var pv in productVariantDTOs)
            {
                var productExists = await _productRepo.ExistsAsync(pv.ProductId);

                if (!productExists)
                    throw new KeyNotFoundException($"ProductVariants ProductID {pv.ProductId} was not found.");

                productVariantsToAdd.Add(new ProductVariant()
                {
                    ProductId = pv.ProductId,
                    Sku = pv.Sku,
                    Color = pv.Color,
                    Price = pv.Price,

                });
            }
            await _productVariantRepo.BulkCreateAsync(productVariantsToAdd);

            return productVariantsToAdd.Count;
        }
    }
}
