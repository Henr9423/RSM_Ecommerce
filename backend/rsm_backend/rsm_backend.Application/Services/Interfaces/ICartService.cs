using rsm_backend.Application.DTO;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface ICartService
    {

        public Task<Cart> GetOrCreateGuestCartAsync(Guid? guestCartId);

        public Task<List<CartItem>> GetCartItemsAsync(int cartId);

        public Task UpdateQuantityAsync(int cartId, int productVariantId, int quantity);



        public Task AddItemToCart(int cartId, int productVariantId, int quantity);
    }
}
