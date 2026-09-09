using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository (AppDbContext dbContext, ILogger<ProductRepository> logger) 
        {
            _context = dbContext;
            _logger = logger;
        }
        public Task AddAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task BulkCreateAsync(List<Product> products)
        {
             await _context.AddRangeAsync(products);
             await _context.SaveChangesAsync();
            
        }

        public async Task BulkCreateProductTagsAsync(List<ProductTag> productTags)
        {
           
                await _context.AddRangeAsync(productTags);
                await _context.SaveChangesAsync();
            
        }

        public async Task<bool> ExistProductTagAsync(int productId ,int tagId)
        {
            return await _context.ProductTags
                .AnyAsync(pt => pt.TagId == tagId && pt.ProductID== productId );

        }

        public async Task<bool> ExistsAsync(int id)
        {
         
           return await _context.Products
                .AnyAsync(p=>p.Id == id);
        }

      

        public Task<Product?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetProductsWithSearch(string? search)
        {
           var query=_context.Products.AsQueryable();

           query= query.Include(p => p.ProductVariants)
                            .ThenInclude(v => v.ProductImages)
                        .Include(p => p.ProductTags)
                            .ThenInclude(pt => pt.Tag).AsSplitQuery();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search=search.Trim();

                //PostgresSql specific to make sure its a case-insensitive search
                query = query.Where(p =>
                EF.Functions.ILike(p.Name, $"%{search}%") ||
                 EF.Functions.ILike(p.Description, $"%{search}%") ||
                p.ProductTags.Any(t => EF.Functions.ILike(t.Tag.Name, $"%{search}%"))
                );

              
            }

            var products= await query.ToListAsync();

            return products;
        }
    }
}
