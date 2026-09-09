using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class UpdateDeliveryOptionDTO
    {
        [Range(0,int.MaxValue)]
        public int DeliveryOptionId { get; set; }
    }
}
