using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class Product
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public int BrandId { get; set; }

		public Brand Brand { get; set; } = null!;

		 public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
			public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();


		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}
