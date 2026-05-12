using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class ProductCardDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
     
        public RatingDTO Rating { get; set; } = null!;

        [Required]
        public decimal? Price { get; set; }

        public List<string> Keywords { get; set; } = new List<string>();

        public string? ImageKey { get; set; }

    }
}
