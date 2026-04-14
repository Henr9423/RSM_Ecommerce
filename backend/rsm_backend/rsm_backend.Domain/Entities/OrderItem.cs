using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class OrderItem
	{
		public int Id { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; } = null!;

		public int ProductVariantId { get; set; }

		public ProductVariant ProductVariant { get; set; } = null!;

		public int Quantity { get; set; }

		public decimal UnitPrice { get; set; }
		public decimal Discount { get; set; }

		public decimal LineTotal { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
