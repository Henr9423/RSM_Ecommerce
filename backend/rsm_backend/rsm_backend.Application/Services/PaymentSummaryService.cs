using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services
{
    public class PaymentSummaryService : IPaymentSummaryService
    {

        private readonly IDiscountService _discountService;

        public PaymentSummaryService(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<PaymentSummaryDTO> GetPaymentSummaryDTOAsync(Cart cart, string? couponCode)
        {
            ArgumentNullException.ThrowIfNull(cart);

           

            if (cart.DeliveryOption is null)
            {
                throw new InvalidOperationException(
                    $"Cart {cart.Id} must have a delivery option.");
            }

            if (cart.Items.Count == 0)
            {
                return new PaymentSummaryDTO()
                {
                    ItemsCount = 0,
                    ProductCost = 0m,
                    ShippingCost = 0m,
                    TotalCostBeforeTax = 0m,
                    Tax = 0m,
                    CouponDiscount = 0,
                    TotalCost = 0

                };
            }

            foreach (var item in cart.Items)
            {
                if (item.ProductVariant is null)
                {
                    throw new InvalidOperationException(
                        $"Product variant was not loaded for cart item {item.Id}.");
                }

            }

            var itemsCount = cart.Items.Sum(item=>item.Quantity);

            var subtotal = cart.Items.Sum(item => (item.UnitPrice - item.ProductVariant.DiscountAmount) *item.Quantity);

            var shippingCost = cart.DeliveryOption.Price;

            var couponDiscount = await _discountService
                .CalculateDiscountAsync(subtotal, couponCode);

            var totalBeforeTax =Math.Max(0m, subtotal + shippingCost - couponDiscount.Amount);

            var tax = 0m;

            var total = totalBeforeTax + tax ;


            return new PaymentSummaryDTO()
            {
                ItemsCount = itemsCount,
                ProductCost = subtotal,
                ShippingCost = shippingCost,
                TotalCostBeforeTax = totalBeforeTax,
                Tax = tax,
                CouponDiscount = couponDiscount.Amount,
                TotalCost = total

            };
        }
    }
}
