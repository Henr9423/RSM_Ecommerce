using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBrandController:ControllerBase
    {
        private readonly IAdminBrandService _adminBrand;
        public AdminBrandController(IAdminBrandService adminBrandService)
        {

            _adminBrand=adminBrandService;

        }



        [HttpPost("bulk")]
        public async Task<IActionResult>  AddBulkBrands([FromBody] List<CreateBrandDTO> brandsToCreate)
        {


            var NumberOfBrandsCreated = await _adminBrand.BulkCreateAsync(brandsToCreate);

            return Ok($"Added {NumberOfBrandsCreated} to the db ");
        }
    }
}
