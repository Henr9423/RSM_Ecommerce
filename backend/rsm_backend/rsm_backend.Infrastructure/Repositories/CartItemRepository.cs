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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddCartItemsAsync(List<CartItem> cartItems)
        {
            throw new NotImplementedException();
        }

        public async Task AddItemAsync(int cartId, int productVariantId, int quantity)
        {
          
            var cartItem = new CartItem() { CartId = cartId, ProductVariantId = productVariantId, Quantity = quantity, CreatedAt=DateTime.UtcNow };

            _context.CartItems.Add(cartItem);
            
        }

     
        public async Task<CartItem?> FindItemByIdAsync(int cartId, int CartItemId)
        {
           var cartItem= await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == CartItemId && ci.CartId==cartId);
           
            return cartItem;
        }

        public async Task<CartItem?> FindItemByVariantIdAsync(int cartId, int productVariantId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductVariantId == productVariantId && ci.CartId == cartId);

            return cartItem;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int cartId)
        {
            return await _context.CartItems.Where(ci=>ci.CartId == cartId).ToListAsync();
        }

        public async Task MergeItemsAsync(int fromCartId, int intoCartId)
        {
            if (fromCartId == intoCartId)
                return;


            List<CartItem> fromItems=await _context.CartItems.Where(ci=>ci.CartId==fromCartId).ToListAsync();

            List<CartItem> intoItems = await _context.CartItems.Where(ci => ci.CartId == intoCartId).ToListAsync();


            foreach (var fromItem in fromItems)
            {
                var existingItem = intoItems.FirstOrDefault(ci => ci.ProductVariantId == fromItem.ProductVariantId);

                if(existingItem!=null)
                {
                    existingItem.Quantity +=fromItem.Quantity;
                    _context.CartItems.Remove(fromItem);
                }
                else
                {
                    fromItem.CartId = intoCartId;
                }
            }

            
        }

        public async Task RemoveItemAsync(CartItem cartItem)
        {
            
            if (cartItem == null)
            { 
                throw new ArgumentNullException(nameof(cartItem));
            }

            _context.Remove(cartItem);

            
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
