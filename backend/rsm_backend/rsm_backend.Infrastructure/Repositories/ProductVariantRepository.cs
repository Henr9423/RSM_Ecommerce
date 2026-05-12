using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using rsm_backend.Application.Services.Interfaces.IRepositories;
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

        public async Task BulkCreateAsync(List<ProductVariant> productVariants)
        {
             await _context.AddRangeAsync(productVariants);
             await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ProductVariants.AnyAsync(x => x.Id == id);
        }

        public Task<List<ProductVariant>> GetAllProductVariants()
        {
            throw new NotImplementedException();
        }

        public Task<ProductVariant?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductVariant?> GetBySkuAsync(string sku)
        {
            throw new NotImplementedException();
        }
    }
}
