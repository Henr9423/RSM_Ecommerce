using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.IRepositories
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetByIdAsync(int id);

        Task<ProductVariant?> GetBySkuAsync(string sku);

        Task<List<ProductVariant>> GetAllProductVariants();

        Task AddAsync(ProductVariant productVariant);

        Task BulkCreateAsync(List<ProductVariant> productVariants);

        Task<bool> ExistsAsync(int id);

      

    }
}
