using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;

namespace rsm_backend.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminProductVariantController:ControllerBase
    {

        private readonly IAdminProductVariantService _adminProductVariantService;
        public AdminProductVariantController(IAdminProductVariantService productVariantService)
        {
            _adminProductVariantService = productVariantService;
        }

        
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBulkProductVariants(List<CreateProductVariantDTO> productVariantDTOs)
        {
            var numberOfPVAdded= await _adminProductVariantService.BulkCreateAsync(productVariantDTOs);
            
            return Ok($"Added {numberOfPVAdded} Product variants to the Db");
        }
    }
}
