using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class ProductImage
	{
		public int Id { get; set; }
		public int ProductVariantId { get; set; }
		public ProductVariant ProductVariant { get; set; } = null!;

		public string StorageKey { get; set; } = string.Empty;

		public string? AltText { get; set; }

		public bool IsPrimary { get; set; } = false;

		public int SortOrder { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


	}
}
