using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services
{
    public class CartService : ICartService
    {

        private readonly ICartItemRepository _cartItemRepo;
        private readonly ICartRepository _cartRepo;

        public CartService (ICartItemRepository cartItemRepo, ICartRepository cartRepo)
        {
            _cartItemRepo = cartItemRepo;
            _cartRepo = cartRepo;
        }

        public async Task AddItemToCart(int cartId, int productVariantId, int quantity)
        {

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var cart =await _cartRepo.FindAsync(cartId);

            if (cart == null)
            {
                throw new KeyNotFoundException($"Could not find cart in db with cartId {cartId}");
            }

            if (cart.Status != CartStatus.active)
                throw new InvalidOperationException("Cannot add items to a non-active cart");

            var existingItem=await _cartItemRepo.FindItemByVariantIdAsync(cartId,productVariantId);

            if(existingItem!=null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                await _cartItemRepo.AddItemAsync(cartId, productVariantId, quantity);

            }

            cart.UpdatedAt = DateTime.UtcNow;

            await _cartRepo.SaveChangesAsync();

        }

        public async Task<List<CartItem>> GetCartItemsAsync(int cartId)
        {

            var cart = await _cartRepo.FindAsync(cartId);

            if (cart == null)
            {
                throw new KeyNotFoundException($"Could not find cart in db with cartId {cartId}");
            }

            List<CartItem> cartItems= await _cartItemRepo.GetCartItemsAsync(cartId);

            return cartItems;

        }

        public async Task<Cart> GetOrCreateGuestCartAsync(Guid? guestCartId)
        {
           
           
            Cart? cart = null;
            
            if (guestCartId.HasValue)
            { 
               cart=await _cartRepo.GetByGuestCartIdAsync(guestCartId.Value);
            }

            if (cart == null)
            {
                cart = new Cart()
                {
                    GuestCartToken = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Status = CartStatus.active
                };

                await _cartRepo.AddAsync(cart);
                await _cartRepo.SaveChangesAsync();
            }

            return cart;
        }

       

        public async Task UpdateQuantityAsync(int cartId, int cartItemId, int quantity)
        {

            var cart = await _cartRepo.FindAsync(cartId);

            if (cart == null)
            {
                throw new KeyNotFoundException($"Could not find cart in db with cartId {cartId}");
            }
            
            if (cart.Status != CartStatus.active)
                throw new InvalidOperationException("Cannot update items in a non-active cart");

            var cartItem = await _cartItemRepo.FindItemByIdAsync(cartId, cartItemId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException($"Could not find cart item with id {cartItemId}");
            }

            if (quantity <= 0)
            {
                await _cartItemRepo.RemoveItemAsync(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _cartItemRepo.SaveChangesAsync();
            
        }
    }
}
