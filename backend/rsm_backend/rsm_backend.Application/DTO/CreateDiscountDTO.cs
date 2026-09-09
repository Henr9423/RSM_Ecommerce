using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class CreateDiscountDTO
    {
        public string Code { get; set; } = "";

        public DiscountType Type { get; set; } // Percentage, FixedAmount

        [Range(typeof(decimal), "0.01", "999999.99")]
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
