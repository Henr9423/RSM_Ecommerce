using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {

        private readonly AppDbContext _context;
        private readonly ILogger<ProductVariantRepository> _logger;

        public ProductVariantRepository(AppDbContext dbContext, ILogger<ProductVariantRepository> logger)
        {
            _context = dbContext;
            _logger = logger;
        }
        public Task AddAsync(ProductVariant productVariant)
        {
            throw new NotImplementedException();
        }

        public async Task AddProductImageToVariantAsync(ProductImage image, int productVariantId, CancellationToken cancellationToken)
        {
           var productVariant= await _context.ProductVariants.FirstOrDefaultAsync(pv=> pv.Id == productVariantId, cancellationToken);

            if (productVariant == null)
            {
                throw new KeyNotFoundException(
                    $"Product variant with ID {productVariantId} was not found.");
            }

            productVariant.ProductImages.Add(image);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task BulkCreateAsync(List<ProductVariant> productVariants)
        {
             await _context.AddRangeAsync(productVariants);
             await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ProductVariants.AnyAsync(x => x.Id == id);
        }

        public async Task<List<ProductVariant>> GetAllProductVariantsAsync()
        {
            return await _context.ProductVariants.ToListAsync();
        }

        public Task<ProductVariant?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductVariant?> GetBySkuAsync(string sku)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductVariant>> GetSpecificProductVariantsAsync(List<int> productVariantIds)
        {
            return await _context.ProductVariants.Where(x => productVariantIds.Contains(x.Id)).ToListAsync();
        }
    }
}
