using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class CreateProductImageDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductVariantId { get; set; }

        [Required]
        public string StorageKey { get; set; } = string.Empty;

        public string? AltText { get; set; }

        public bool IsPrimary { get; set; } = false;
        
 
    }
}
