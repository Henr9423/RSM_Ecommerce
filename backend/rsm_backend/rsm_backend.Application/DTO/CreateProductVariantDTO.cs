using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class CreateProductVariantDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        public string Sku { get; set; } = string.Empty;

        
        public string? Color { get; set; }

       
        [Range(0.01, 999999)]
        public decimal Price { get; set; }


    }
}
