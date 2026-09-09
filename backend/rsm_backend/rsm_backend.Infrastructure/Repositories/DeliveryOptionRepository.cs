using Microsoft.EntityFrameworkCore;
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
    public class DeliveryOptionRepository : IDeliveryOptionRepository
    {
        private readonly AppDbContext _context;

        public DeliveryOptionRepository (AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(DeliveryOption deliveryOption)
        {
            throw new NotImplementedException();
        }

        public async Task BulkCreateAsync(List<DeliveryOption> deliveryOptions)
        {
           await _context.AddRangeAsync(deliveryOptions);
        }

        public async Task<bool> ExistsAsync(int deliveryOptionId)
        {
           return await _context.DeliveryOptions.AnyAsync(x => x.Id == deliveryOptionId);
        }

        public async Task<List<DeliveryOption>> GetAllDeliveryOptionsAsync()
        {
            return await _context.DeliveryOptions.ToListAsync();
        }

        public Task<DeliveryOption?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DeliveryOption?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
