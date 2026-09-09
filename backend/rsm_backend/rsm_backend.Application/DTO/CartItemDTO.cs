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
        public int Id { get; set; }


        [Required]
        public int ProductVariantId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public string ProductName { get; set; } = "";

        [Range(typeof(decimal), "0.01", "999999.99")]
        public decimal UnitPrice { get; set; }

        public string? ImageUrl { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999.99")]
        public decimal LineTotal { get; set; }

      
        
    }
}
