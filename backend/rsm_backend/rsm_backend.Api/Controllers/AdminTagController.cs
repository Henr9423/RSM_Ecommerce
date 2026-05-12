using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;

namespace rsm_backend.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminTagController:ControllerBase
    {
        private IAdminTagService _adminTag;

        public AdminTagController(IAdminTagService adminTagService)
        {
            _adminTag = adminTagService;

        }

        [HttpPost("bulk")]
        public async Task <IActionResult> AddBulkTags([FromBody] List<CreateTagDTO> createTagDTOs )
        {

            var numberOfTagsCreated = await _adminTag.BulkCreateTagsAsync(createTagDTOs);

            return Ok($"Added {numberOfTagsCreated} to the db ");
        }
    }
}
