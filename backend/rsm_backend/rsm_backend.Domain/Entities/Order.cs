using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public enum OrderStatus
	{
		Pending,
		Paid,
		Processing,
		Shipped,
		Delivered,
		Cancelled
	}
	public class Order
	{
		public int Id { get; set; }

        public string OrderNumber { get; set; } = null!;

        // customer (FK)
        public int CustomerId { get; set; }
		public Customer Customer { get; set; } = null!;

		// addresses (FKs)
		public int? ShippingAddressId { get; set; }
		public OrderAddress? ShippingAddress { get; set; }

		public int? BillingAddressId { get; set; }
		public OrderAddress? BillingAddress { get; set; }


		public OrderStatus Status { get; set; }


		public decimal ShippingFee { get; set; }

        public int? DeliveryOptionId { get; set; }

        public DeliveryOption? DeliveryOption { get; set; }


        public DateTime? EstimatedDeliveryFrom { get; set; }
        public DateTime? EstimatedDeliveryTo { get; set; }

        public decimal Subtotal {  get; set; }

		public decimal Tax {  get; set; }

		public decimal CouponDiscount { get; set; }
		public decimal Total { get; set; }

        public GuestOrderVerification? GuestOrderVerification { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<OrderItem> OrderItems { get; set; }= new List<OrderItem>();

		public ICollection<Payment> Payments { get; set; }=new List<Payment>();

	}
}
