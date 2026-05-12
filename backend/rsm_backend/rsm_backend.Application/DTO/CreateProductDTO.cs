using rsm_backend.Application;
using System.ComponentModel.DataAnnotations;

namespace rsm_backend.Application.DTO
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public int? BrandId { get; set; }

    }
}
