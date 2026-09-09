using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IDeliveryOptionRepository _deliveryOptionRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IObjectStorage _objectStorage;

        public CartService (ICartItemRepository cartItemRepo, ICartRepository cartRepo, IDeliveryOptionRepository deliveryOptionRepo, IHttpContextAccessor httpContextAccessor, IObjectStorage objectStorage)
        {
            _cartItemRepo = cartItemRepo;
            _cartRepo = cartRepo;
            _deliveryOptionRepo = deliveryOptionRepo; 
            _httpContextAccessor= httpContextAccessor;
            _objectStorage = objectStorage;
        }


        private CartDTO ToCartDto(Cart cart)
        {
           
            return new CartDTO
            {
                Status = cart.Status,

                Items = cart.Items.Select(i => new CartItemDTO
                {
                    Id = i.Id,
                    ProductVariantId = i.ProductVariantId,
                    ProductName = i.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.Quantity * i.UnitPrice,
                    ImageUrl = _objectStorage.GetPublicUrl(i.ProductVariant.ProductImages.FirstOrDefault()?.StorageKey)

                }).ToList(),
                DeliveryOptionId=cart.DeliveryOptionId,
                TotalPrice = cart.Items.Sum(i => i.Quantity * i.UnitPrice),

            };
        }

        private async Task<Cart> GetOrCreateGuestCartAsync()
        {

            var httpContext = _httpContextAccessor.HttpContext
                             ?? throw new InvalidOperationException("HttpContext is not available.");

            var request = _httpContextAccessor.HttpContext.Request;
            var response = _httpContextAccessor.HttpContext.Response;

            var token = request.Cookies["GuestCartToken"];

            if (string.IsNullOrWhiteSpace(token))
            {
                token = Guid.NewGuid().ToString();

                response.Cookies.Append("GuestCartToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
            }

            var guestCart = await _cartRepo.GetOrCreateGuestCartAsync(token);


            return guestCart;

        }

        public async Task AddItemToCartAsync(int productVariantId, int quantity)
        {

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var cart= await GetOrCreateCurrentCartAsync();

          
            if (cart.Status != CartStatus.Active)
                throw new InvalidOperationException("Cannot add items to a non-active cart");

            var existingItem=cart.Items.FirstOrDefault(ci=>ci.ProductVariantId==productVariantId);

            if(existingItem!=null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                await _cartItemRepo.AddItemAsync(cart.Id, productVariantId, quantity);

            }

            cart.UpdatedAt = DateTime.UtcNow;

            await _cartRepo.SaveChangesAsync();

        }

        public async Task<Cart> GetOrCreateCurrentCartAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext
                            ?? throw new InvalidOperationException("HttpContext is not available.");


            if (_httpContextAccessor.HttpContext!.User.Identity?.IsAuthenticated == true)
            {
                var userId = _httpContextAccessor.HttpContext.User
                              .FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException();

                var userCart = await _cartRepo.GetByUserIdAsync(userId);

                return userCart;
            }


            return await GetOrCreateGuestCartAsync(); 
        }


        public async Task<CartDTO> GetCurrentCartDtoAsync()
        {
            var cart = await GetOrCreateCurrentCartAsync();

            return ToCartDto(cart);
        }

       

        public async Task UpdateDeliveryOptionAsync(int deliveryOptionId)
        {
            var cart = await GetOrCreateCurrentCartAsync();

            var deliveryOptionExists = await _deliveryOptionRepo.ExistsAsync(deliveryOptionId);
                                                      
            if (!deliveryOptionExists)
                throw new Exception("Delivery option not found.");


            cart.DeliveryOptionId = deliveryOptionId;

            cart.UpdatedAt = DateTime.UtcNow;

            await _deliveryOptionRepo.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {

            var cart = await GetOrCreateCurrentCartAsync();

         
            if (cart.Status != CartStatus.Active)
                throw new InvalidOperationException("Cannot update items in a non-active cart");

           var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);

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

        public async Task DeleteItemAsync(int cartItemId)
        {
            var cart= await GetOrCreateCurrentCartAsync();

            var cartItemToDelete = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);

            if(cartItemToDelete==null)
            {
                throw new KeyNotFoundException($"Delete item failed due to cartItemId {cartItemId} is not in the cart");
            }


            await _cartItemRepo.RemoveItemAsync(cartItemToDelete);

            cart.UpdatedAt = DateTime.UtcNow;

            await _cartItemRepo.SaveChangesAsync();

        }
    }
}
