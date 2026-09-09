using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class UpdateCartItemQuantityDTO
    {

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
