using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using rsm_backend.Application.Services.Interfaces.IRepositories;
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
    public class BrandRepository : IBrandRepository   
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<BrandRepository> _logger;

        public BrandRepository (AppDbContext appDbContext, ILogger<BrandRepository> logger)
        {
            _dbContext = appDbContext;
            _logger= logger;
        }

        public async Task AddAsync(Brand brand)
        {
            if (brand == null)
                throw new ArgumentNullException(nameof(brand));
            try
            {
                await _dbContext.Brands.AddAsync(brand);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding brand");
                throw;
            }
        
        }

        public async Task BulkCreateAsync(List<Brand> brands)
        {
            if (brands == null)
                throw new ArgumentNullException();


            if (brands.Count == 0)
                return;

            try
            {
                await _dbContext.Brands.AddRangeAsync(brands);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding bulk of brands. Count: {Count}", brands.Count);
                throw;
            }
          
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
           if(id<=0)
            {
                throw new ArgumentException("Id must be greater than 0",nameof(id));
            }
            

           return await _dbContext.Brands.FindAsync(id);

            
        }

        public async Task<Brand?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            return await _dbContext.Brands.FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
