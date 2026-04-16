using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class Customer
	{
		public int Id { get; set; }

		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;

		// Default addresses (FKs)
		public int? DefaultShippingAddressId { get; set; }
		public CustomerAddress? DefaultShippingAddress { get; set; }

		public int? DefaultBillingAddressId { get; set; }
		public CustomerAddress? DefaultBillingAddress { get; set; }

		// All addresses
		public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();

		// All orders
		public ICollection<Order> Orders { get; set; } = new List<Order>();

		// All carts
		public ICollection<Cart> Carts { get; set; }=new List<Cart>();

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}
