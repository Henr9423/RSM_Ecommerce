using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    public class DeliveryOptionsController:ControllerBase
    {
        private readonly IAdminDeliveryOptionService _adminDeliveryService;


        public DeliveryOptionsController (IAdminDeliveryOptionService adminDeliveryService)
        {
            _adminDeliveryService = adminDeliveryService;
        }

        [HttpPost("bulk")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> BulkCreate([FromBody] List<CreateDeliveryOptionDTO> createDeliveryOptionDTOs)
        {
            if (createDeliveryOptionDTOs.Count == 0)
            {
                return BadRequest("At least one delivery option is required.");
            }
           

            await _adminDeliveryService.BulkCreate(createDeliveryOptionDTOs);

            return Created("","Created deliveryOptions");
        }

        [HttpGet]
        public async Task<ActionResult<List<DeliveryOptionDTO>>> GetDeliveryOptions()
        {

           var options= await _adminDeliveryService.GetAllDeliveryOptions();

            return Ok(options);
        }
    }
}
