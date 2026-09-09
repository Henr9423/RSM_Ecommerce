using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class PlaceOrderDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string AddressLine1 {  get; set; } = string.Empty;
       
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;

        public string? StateOrRegion { get; set; }

        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? CouponCode { get; set; }

        public bool BillingSameAsShipping { get; set; }

        public string? BillingAddressLine1 { get; set; }
        public string? BillingAddressLine2 { get; set; }

        public string? BillingCity { get; set; }
        public string? BillingStateOrRegion { get; set; }

        public string? BillingPostalCode { get; set; }
        public string? BillingCountry { get; set; }

    }
}
