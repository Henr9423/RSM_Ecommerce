using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface ICartItemRepository
    {

        public Task<CartItem?> FindItemByIdAsync(int cartId, int cartItemId);

        public Task<CartItem?> FindItemByVariantIdAsync(int cartId, int productVariantId);

        public Task AddItemAsync(int cartId, int productVariantId, int quantity);

        public Task UpdateDeliveryOptionAsync( int cartId ,int cartItemId, int deliveryOptionId);

        public Task SaveChangesAsync();
        public Task RemoveItemAsync(CartItem cartItem);
        public Task<List<CartItem>> GetCartItemsAsync(int cartId);

        public Task AddCartItemsAsync(List<CartItem> cartItems);

        public Task MergeItemsAsync(int fromCartId, int intoCartId);

    }
}
