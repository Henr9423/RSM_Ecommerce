using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class Category
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();


		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	}
}
