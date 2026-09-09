using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController:ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController (IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> CreateBulkDiscounts([FromBody] List<CreateDiscountDTO> createDiscountDTOs)
        {
            await _discountService.BulkCreate(createDiscountDTOs);

            return Created("", $"Created {createDiscountDTOs.Count} Discounts");
        }
        
    }
}
