using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetByIdAsync(int id);

        Task<ProductVariant?> GetBySkuAsync(string sku);

        Task<List<ProductVariant>> GetAllProductVariantsAsync();

        Task<List<ProductVariant>> GetSpecificProductVariantsAsync(List<int> productVariantIds);

        Task AddAsync(ProductVariant productVariant);

        Task AddProductImageToVariantAsync(ProductImage image, int productVariantId, CancellationToken cancellationToken);

        Task BulkCreateAsync(List<ProductVariant> productVariants);

        Task<bool> ExistsAsync(int id);

      

    }
}
