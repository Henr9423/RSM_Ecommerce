using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);

        Task<Customer?> GetByUserIdAsync(string userId);

        Task<Customer?> GetByNameAsync(string name);

        Task<List<Customer>> GetAllCustomers();

        Task AddAsync(Customer product);

        Task BulkCreateAsync(List<Customer> customers);

        Task<bool> ExistsAsync(int id);

        Task<bool> ExistProductTagAsync(int productId, int tagId);
    }
}
