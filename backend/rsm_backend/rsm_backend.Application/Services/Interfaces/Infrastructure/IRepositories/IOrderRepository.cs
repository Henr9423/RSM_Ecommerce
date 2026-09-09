using rsm_backend.Application.DTO;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface IOrderRepository
    {
        public Task<Order> AddOrderAsync(Order order);

        public Task<List<Order>> GetAllByUserIdAsync(string userId);

        public Task<Order?> GetByUserId(int orderId, string userId);

        public Task<Order?> GetByGuestAccessToken(string guestAccessTokenHash, int orderId);

        public Task<Order?> GetByOrderNumber(string orderNumber);

     

        public Task CancelOrder(int orderId, string? userId, string? guestAccessToken);

        public Task UpdateStatus(OrderStatus status);


        public Task SaveChangesAsync();

        public Task<Order?> FindAsync(int orderId);

    }
}
