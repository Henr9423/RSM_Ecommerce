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

        public Task UpdateQuantityAsync(int cartItemId, int quantity);

        public Task UpdateDeliveryOptionAsync(int DeliveryOptionId);


        public Task AddItemToCartAsync(int productVariantId, int quantity);

        public Task<Cart> GetOrCreateCurrentCartAsync();

        public Task<CartDTO> GetCurrentCartDtoAsync();

        public Task DeleteItemAsync(int cartItemId);


    }
}
