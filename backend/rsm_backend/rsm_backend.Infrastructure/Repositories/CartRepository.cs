using Microsoft.AspNetCore.Http;
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
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
      
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(AppDbContext context, IHttpContextAccessor httpContext )
        {
            _context = context;
           
            _httpContextAccessor = httpContext;
        }

        private async Task<Cart?> GetByOwnerAsync(string? userId, string? guestToken)
        {
            IQueryable<Cart> query = _context.Carts.Include(c => c.Items);
            
            if(userId!=null)
            {
                return await query.FirstOrDefaultAsync(c => c.Customer != null && c.Customer.UserId == userId);
            }

            if(guestToken!=null)
            {
                return await query.FirstOrDefaultAsync(c => c.GuestCartToken == guestToken);
            }

            return null;
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }


        public async Task<Cart> CreateForGuestAsync(string guestId)
        {
            var cart = new Cart() { Status = CartStatus.Active, CreatedAt = DateTime.UtcNow, GuestCartToken = guestId };

            await _context.Carts.AddAsync(cart);

            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task DeleteItemAsync(string? userId, string? guestToken, int productVariantId)
        {
           var cart= await GetByOwnerAsync(userId, guestToken);

            if (cart == null)
                return;

            var item = cart.Items.FirstOrDefault(i => i.ProductVariantId == productVariantId);

            if (item == null)
                return;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> FindAsync(int cartId)
        {
            return await _context.Carts.FindAsync(cartId);
        }

        public async Task<Cart> GetOrCreateGuestCartAsync(string guestCartId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(guestCartId);

            var cart = await _context.Carts.Include(c=>c.Items).ThenInclude(ci => ci.ProductVariant).ThenInclude(pv=>pv.ProductImages).Include(ci=>ci.DeliveryOption).Include(c=>c.Customer)
                .FirstOrDefaultAsync(c => c.GuestCartToken == guestCartId && c.Status==CartStatus.Active);


            if (cart != null)
            {
                return cart;
            }

            var defaultDeliveryOption = await _context.DeliveryOptions.OrderBy(x => x.Id).FirstOrDefaultAsync();

            if(defaultDeliveryOption==null)
            {
                throw new InvalidOperationException("Cannot create a cart because no delivery options have been configured.");
            }

            cart = new Cart() { Status = CartStatus.Active, CreatedAt = DateTime.UtcNow, GuestCartToken = guestCartId, DeliveryOptionId=defaultDeliveryOption.Id};

            await AddAsync(cart);

            await SaveChangesAsync();


            return cart;
        }

        public async Task<Cart> GetByUserIdAsync(string userId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            var userCart = await _context.Carts.Include(c=>c.Items).ThenInclude(ci=>ci.ProductVariant).ThenInclude(pv => pv.ProductImages).Include(ci => ci.DeliveryOption).Include(c=>c.Customer)
                .FirstOrDefaultAsync(c => c.Customer!=null && c.Customer!.UserId== userId && c.Status==CartStatus.Active);

            if (userCart != null)
                return userCart;

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Could not find a customer with the userId {userId}");
            }


            Cart cart = new Cart()
            {
                CustomerId = customer.Id,
                CreatedAt = DateTime.UtcNow,
                Status = CartStatus.Active

            };

            await AddAsync(cart);
            await SaveChangesAsync();

            return cart;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}
