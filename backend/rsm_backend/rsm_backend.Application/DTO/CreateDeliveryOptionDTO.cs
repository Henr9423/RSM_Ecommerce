using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class CreateDeliveryOptionDTO
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1,int.MaxValue)]
       public int MinDeliveryDays { get; set; }


        [Range(1, int.MaxValue)]
        public int MaxDeliveryDays { get; set; }

        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

    }
}
