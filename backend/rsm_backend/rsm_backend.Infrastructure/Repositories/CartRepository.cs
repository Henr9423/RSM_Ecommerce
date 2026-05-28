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
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task<Cart?> FindAsync(int cartId)
        {
            return await _context.Carts.FindAsync(cartId);
        }

        public async Task<Cart?> GetByGuestCartIdAsync(Guid guestCartId)
        {

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.GuestCartToken == guestCartId);


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
