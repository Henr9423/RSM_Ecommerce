using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class ProductVariant
	{
		public int Id { get; set; }

		public int ProductId { get; set; }

		public Product Product { get; set; } = null!;

		public string Sku { get; set; } = string.Empty;

		public string Color { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public Inventory Inventory { get; set; } = null!;

		public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}
