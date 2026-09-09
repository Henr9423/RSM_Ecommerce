using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface IDeliveryOptionRepository
    {
        Task<DeliveryOption?> GetByIdAsync(int id);

        Task<DeliveryOption?> GetByNameAsync(string name);

        Task AddAsync(DeliveryOption deliveryOption);

        Task BulkCreateAsync(List<DeliveryOption> deliveryOptions);

        Task<bool> ExistsAsync(int deliveryOptionId);

        Task SaveChangesAsync();

        Task<List<DeliveryOption>> GetAllDeliveryOptionsAsync();
    }
}
