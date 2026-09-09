using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class PaymentSummaryDTO
    {
        public int  ItemsCount {  get; set; }
        public decimal ProductCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalCostBeforeTax { get; set; }
        public decimal Tax { get; set; }
        public decimal CouponDiscount { get; set; }
        public decimal TotalCost{ get; set; }
 
    }
}
