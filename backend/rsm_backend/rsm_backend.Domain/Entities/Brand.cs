using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class Brand
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public ICollection<Product> Products { get; set; }=new List<Product>();

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	}
}
