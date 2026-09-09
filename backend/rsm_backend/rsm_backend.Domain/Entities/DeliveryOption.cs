using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
    public class DeliveryOption
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int MinDeliveryDays { get; set; }
        public int MaxDeliveryDays { get; set; }

        public decimal Price { get; set; }

        public ICollection<Cart> Carts { get; set; }=new List<Cart>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
