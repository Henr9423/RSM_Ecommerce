using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{

	public enum CartStatus
	{
		active,
		checked_out,
		abandoned,
		expired

	}
	public class Cart
	{
		public int Id { get; set; }


        // null for guest carts
        public int? CustomerId { get; set; }
		public Customer? Customer { get; set; }

		// null for authenticated carts
		public Guid? GuestCartToken { get; set; }

		public CartStatus Status { get; set; }

		public ICollection<CartItem> Items { get; set; }= new List<CartItem>();

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	}
}
