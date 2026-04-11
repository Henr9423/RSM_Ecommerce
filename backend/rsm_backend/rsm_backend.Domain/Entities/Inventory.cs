using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class Inventory
	{

		public int ProductVariantId { get; set; }

		public ProductVariant ProductVariant { get; set; } = null!;

		public int QuantityInStock { get; set; }

		public int ReservedQuantity { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	}
}
