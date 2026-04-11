using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
	public class CustomerAddress
	{
		public int Id { get; set; }

		public int CustomerId { get; set; }

		public Customer Customer { get; set; } = null!;
		public string FullName { get; set; } = string.Empty;
		public string AddressLine1 { get; set; } = string.Empty;
		public string? AddressLine2 { get; set; }

		public string City { get; set; } = string.Empty;

		public string? StateOrRegion { get; set; }

		public string PostalCode { get; set; } = string.Empty;

		public string Country { get; set; } = string.Empty;

		public string PhoneNumber { get; set; } = string.Empty;

		public bool IsDefault { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


	}
}
