using Microsoft.EntityFrameworkCore;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;


        public OrderRepository(AppDbContext dbContext) 
        {
            _context = dbContext;
          
        }
        public async Task<Order> AddOrderAsync(Order order)
        {

            try
            {
                _context.Orders.Add(order);
               

                return order;
            }
            catch (DbUpdateException ex)
            {
                var databaseMessage =
                    ex.InnerException?.Message ?? ex.Message;

                throw new InvalidOperationException(
                    $"Failed to save the order: {databaseMessage}",
                    ex);
            }
        }

       

        public Task CancelOrder(int orderId, string? userId, string? guestAccessToken)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> FindAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetByGuestAccessToken(string guestAccessTokenHash, int orderId)
        {
                 return await _context.Orders
                 .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.ProductImages)
                 .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                 .Include(o=> o.GuestOrderVerification)
                 .Include(o => o.Customer)
                 .AsSplitQuery()
                 .SingleOrDefaultAsync(o => o.GuestOrderVerification!=null && o.GuestOrderVerification.CodeHash==guestAccessTokenHash && o.Id== orderId);
        }

        public async Task<List<Order>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Orders
                 .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.ProductImages)
                 .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                 .Include(o => o.Customer)
                 .AsSplitQuery()
                 .Where(o => o.Customer.UserId == userId).ToListAsync();
                
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateStatus(OrderStatus status)
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetByUserId(int orderId, string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.ProductImages)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(o => o.Customer)
                .AsSplitQuery()
                .SingleOrDefaultAsync(o => o.Customer.UserId == userId);
        }

        public async Task<Order?> GetByOrderNumber(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.ProductImages)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(o=>o.Customer)
                .AsSplitQuery()
                .SingleOrDefaultAsync(o => o.OrderNumber==orderNumber);
        }

      

        
    }
}
