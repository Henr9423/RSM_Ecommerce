using Microsoft.EntityFrameworkCore;
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
    public class ProductImageRepository : IProductImageRepository
    {

        private readonly AppDbContext _context;

        public ProductImageRepository (AppDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(ProductImage productImage)
        {
            throw new NotImplementedException();
        }

        public async Task BulkCreateAsync(List<ProductImage> productImages)
        {
            await _context.AddRangeAsync(productImages);
            await _context.SaveChangesAsync();

        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int productVariantId,string storageKey)
        {
           return await _context.ProductImages.AnyAsync(i => i.ProductVariantId ==productVariantId && i.StorageKey == storageKey);
        }

        public Task<List<ProductImage>> GetAllProductImages()
        {
            throw new NotImplementedException();
        }

        public Task<ProductImage?> GetByAltTextAsync(string altText)
        {
            throw new NotImplementedException();
        }

        public Task<ProductImage?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetNextSortOrderAsync(int productVariantId)
        {
            var maxSortOrder = await _context.ProductImages
                                    .Where(x => x.ProductVariantId == productVariantId)
                                    .MaxAsync(x => (int?)x.SortOrder);

            return (maxSortOrder ?? -1) + 1;
        }
    }
}
