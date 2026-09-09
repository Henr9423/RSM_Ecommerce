using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class AddCartItemDTO
    {
        [Range(0,int.MaxValue)]
        public int ProductVariantId { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
