using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProductImageController:ControllerBase
    {
        private readonly IAdminProductImageService _adminProductImageService;
        public AdminProductImageController(IAdminProductImageService adminProductImageService)
        {
            _adminProductImageService = adminProductImageService;
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<string>> AddBulkProductImages([FromBody] List<CreateProductImageDTO> productImageDTOs)
        {
            var productImagesAdded=await _adminProductImageService.BulkCreateAsync(productImageDTOs);

            return Ok($"Added {productImagesAdded} to the DB");
        }

    }
}
