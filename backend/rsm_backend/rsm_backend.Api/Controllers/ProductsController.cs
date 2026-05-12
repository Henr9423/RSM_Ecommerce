using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Data;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController:ControllerBase
    {
        private IProductService _productService;

        public ProductsController(IProductService productService) 
        {
            _productService =productService ;
        
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts ()
        {
            List<ProductCardDTO> products = await _productService.GetAllProducts();

            return Ok(products);
           
        }

     



    }

}
