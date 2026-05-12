using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProductsController:ControllerBase
    {

        private IAdminProductService _adminProduct;

        public AdminProductsController(IAdminProductService adminProductService)
        {
            _adminProduct = adminProductService;

        }
        [HttpPost("bulk")]
        public async Task<ActionResult<string>> AddBulkProducts([FromBody] List<CreateProductDTO> productsToCreate)
        {


            var numberOfProductsCreated = await _adminProduct.BulkCreateAsync(productsToCreate);

            return Ok($"Added {numberOfProductsCreated} to the db ");
        }

        [HttpPost("product-tags/bulk")]
        public async Task<ActionResult<string>> AddBulkProductTags([FromBody] List<CreateProductTagDTO> productTagsToCreate)
        {
            var productTagsCreated= await _adminProduct.BulkCreateProductTagsAsync(productTagsToCreate);

            return $"Added {productTagsCreated} productTags to the db";
        }
    }
}
