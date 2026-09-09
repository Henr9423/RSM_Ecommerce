using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface ICartRepository
    {

        
        public Task<Cart> GetOrCreateGuestCartAsync(string guestCartId);

        public Task<Cart> GetByUserIdAsync(string userId);

        public Task<Cart> CreateForGuestAsync(string guestId);

        public Task AddAsync (Cart cart);

        public Task DeleteItemAsync(string? userId, string? guestToken, int productVariantId);

        public Task UpdateAsync (Cart cart);


        public Task SaveChangesAsync();

        public Task<Cart?> FindAsync(int cartId);
    }
}
