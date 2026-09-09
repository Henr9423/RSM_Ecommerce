using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

       public DateTime CreatedAt { get; set; }

       public DateTime? EstimatedDeliveryFrom { get; set; }
       public DateTime? EstimatedDeliveryTo { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalCost {  get; set; }

        public string? Email { get; set; }

      public List<OrderItemDTO> OrderItems { get; set; }= new List<OrderItemDTO>();
    }
}
