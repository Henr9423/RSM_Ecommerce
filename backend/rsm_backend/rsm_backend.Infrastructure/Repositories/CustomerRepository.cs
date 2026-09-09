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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context; 
        }

        public Task AddAsync(Customer product)
        {
            throw new NotImplementedException();
        }

        public Task BulkCreateAsync(List<Customer> customers)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistProductTagAsync(int productId, int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> GetByUserIdAsync(string userId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
