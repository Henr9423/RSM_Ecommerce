using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;

namespace rsm_backend.Api.Controllers
{
    public class AdminProductImageController:ControllerBase
    {
        public Task<IActionResult> AddBulkProductImages([FromBody] List<CreateProductImageDTO> productImageDTOs)
        {

        }

    }
}
