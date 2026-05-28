using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class CartItemDTO
    {
        [Required]
        public string CartId { get; set; } = string.Empty;


        [Required]
        public string ProductVariantId { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } 

    }
}
