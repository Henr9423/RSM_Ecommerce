using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        // product object, only when expand=products
        public ProductCardDTO? Product {  get; set; }

    }
}
