using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure;
using rsm_backend.Infrastructure.Data;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController:ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) 
        {
            _productService =productService ;
         
        
        }

         
        // Can handle both with or without a search paramter in the query.
        // If no search value is in the query, no filtering is done and all the products are returned. 
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? search)
        {

            List<ProductCardDTO> products = await _productService.GetProductsAsync(search);

            return Ok(products);

        }

        [HttpPost("{productId}/variants/{productVariantId}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImageToProductVariant( [FromRoute] int productId, [FromRoute] int productVariantId,[FromForm] AddProductImageRequest request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("No image provided.");
            }

            await using var inputStream = request.File.OpenReadStream();

            var result= await _productService.AddProductImageAsync(productId, productVariantId, inputStream, request, cancellationToken);

            return Ok(result);
        }




    }

}
