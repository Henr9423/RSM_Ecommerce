using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class CartDTO
    {
        public CartStatus Status { get; set; }

        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();

        public int? DeliveryOptionId { get; set; }

        public decimal TotalPrice { get; set; }
        

    }
}
