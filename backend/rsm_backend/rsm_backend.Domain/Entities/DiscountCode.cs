using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
    public enum DiscountType {Percentage, FixedAmount}
    public class DiscountCode
    {
        public int Id { get; set; }

        public string Code { get; set; } = "";

        public DiscountType Type { get; set; } // Percentage, FixedAmount
        public decimal Value { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public int? MaxUses { get; set; }        // total usage limit
        public int UsedCount { get; set; }       // how many times used

        public int? MaxUsesPerCustomer { get; set; }

        public decimal? MinimumOrderAmount { get; set; }

    }
}
