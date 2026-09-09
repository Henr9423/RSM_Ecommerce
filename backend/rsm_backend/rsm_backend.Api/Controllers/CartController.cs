using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("items")]
        public async Task<ActionResult> AddItemToCart([FromBody] AddCartItemDTO dto)
        {
            await _cartService.AddItemToCartAsync(dto.ProductVariantId, dto.Quantity);

            return Ok();
        }


        [HttpGet()]
        public async Task<ActionResult<CartDTO>> GetCurrentCart()
        {
            var cartDTO = await _cartService.GetCurrentCartDtoAsync();


            return Ok(cartDTO);
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<ActionResult> DeleteItem(int cartItemId)
        {
            await _cartService.DeleteItemAsync(cartItemId);

            return NoContent();
        }

        [HttpPut("items/{cartItemId}/quantity")]
        public async Task<ActionResult> UpdateQuantity(int cartItemId, [FromBody] UpdateCartItemQuantityDTO dto)
        {
            await _cartService.UpdateQuantityAsync(cartItemId, dto.Quantity);

            return NoContent();

        }

        [HttpPut("items/delivery-option")]
        public async Task<ActionResult> UpdateDeliveryOption([FromBody] UpdateDeliveryOptionDTO dto)
        {
            await _cartService.UpdateDeliveryOptionAsync(dto.DeliveryOptionId);

            return NoContent();

        }



    }
}
