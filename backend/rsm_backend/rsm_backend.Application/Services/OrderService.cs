using Microsoft.AspNetCore.Http;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services
{
    public class OrderService : IOrderService
    {

        private readonly ICartService _cartService;
      
        private readonly IOrderRepository _orderRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentSummaryService _paymentSummaryService;
        private readonly IEmailService _emailService;
        private readonly IObjectStorage _objectStorage;
        public OrderService(ICartService cartService, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IPaymentSummaryService paymentSummaryService, IEmailService emailService, IObjectStorage objectStorage) 
        {
            _cartService = cartService;
            _orderRepo = orderRepository;
            _unitOfWork = unitOfWork;
            _paymentSummaryService = paymentSummaryService;
            _emailService = emailService;
            _objectStorage = objectStorage;
            
        }

        public async Task<PlaceOrderResponseDTO> AddCartToOrderAsync(PlaceOrderDTO dto, CancellationToken cancellationToken=default)
        {


            ArgumentNullException.ThrowIfNull(dto);

            var cart = await _cartService.GetOrCreateCurrentCartAsync();

            if (cart.Items.Count == 0)
            {
                throw new InvalidOperationException(
                    "An empty cart cannot be converted into an order.");
            }

            if (cart.DeliveryOption is null)
            {
                throw new InvalidOperationException(
                    "A cart needs a deliveryoption so the cart can be converted to a valid order.");
            }

            if(dto.BillingSameAsShipping==true)
            {
                dto.BillingAddressLine1 = dto.AddressLine1;
                dto.BillingAddressLine2 = dto.AddressLine2;
                dto.BillingCity = dto.City;
                dto.BillingCountry = dto.Country;
                dto.BillingPostalCode = dto.PostalCode;
                dto.BillingStateOrRegion = dto.StateOrRegion;

            }


            var now = DateTime.UtcNow;

            Customer customer;
            string? guestAccessToken = null;

            if (cart.Customer is not null)
            {
                customer = cart.Customer;
            }
            else
            {
                customer = new Customer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    IsGuest = true,
                    CreatedAt = now
                };

                
            }

            var orderItems = CreateOrderItems(cart, now);

            // This assumes LineTotal already contains product-level discounts.

            var paymentSummary=await _paymentSummaryService.GetPaymentSummaryDTOAsync(cart, dto.CouponCode);
            
            var subtotal = paymentSummary.ProductCost;

            var couponDiscount = paymentSummary.CouponDiscount;

            var shippingFee = paymentSummary.ShippingCost;
                

            var tax = paymentSummary.Tax;

            var shippingAddress = CreateShippingAddress(dto, now);
            var billingAddress = CreateBillingAddress(dto, now);

            var total = paymentSummary.TotalCost;

            if (total < 0)
            {
                total = 0;
            }

            var order = new Order
            {
                Status = OrderStatus.Pending,
                Customer = customer,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                CreatedAt = now,
                GuestOrderVerification = null,
                DeliveryOptionId = cart.DeliveryOptionId,
                EstimatedDeliveryFrom = AddBusinessDays(now, cart.DeliveryOption.MinDeliveryDays),
                EstimatedDeliveryTo = AddBusinessDays(now, cart.DeliveryOption.MaxDeliveryDays),
                Subtotal = subtotal,
                CouponDiscount = couponDiscount,
                ShippingFee = shippingFee,
                Tax = tax,
                Total = total,

                OrderItems = orderItems,
                OrderNumber = GenerateOrderNumber()

            };

            await _orderRepo.AddOrderAsync(order);

            cart.Status = CartStatus.Checked_Out;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (customer.IsGuest==true)
            {
                await _emailService.SendOrderConfirmationAsync(dto.Email, order.OrderNumber);
            }

            return new PlaceOrderResponseDTO() { GuestAccessToken = guestAccessToken, OrderId = order.Id, Total = total };
            
        }




        private static string GenerateOrderNumber()
        {
            return $"RSM-{Guid.NewGuid():N}"[..12].ToUpperInvariant();
        }

        private static List<OrderItem> CreateOrderItems(Cart cart, DateTime createdAt)
        {
            var orderItems = new List<OrderItem>();

            if (cart.DeliveryOptionId is not int deliveryOptionId ||
                  cart.DeliveryOption is null)
            {
                throw new InvalidOperationException(
                    $"Cart {cart.Id} must have a delivery option.");
            }

            foreach (var item in cart.Items)
            {
                if (item.ProductVariant is null)
                {
                    throw new InvalidOperationException(
                        $"Product variant was not loaded for cart item {item.Id}.");
                }

              

                var unitDiscount = item.ProductVariant.DiscountAmount;
                var effectiveUnitPrice = item.UnitPrice - unitDiscount;

                if (effectiveUnitPrice < 0)
                {
                    throw new InvalidOperationException(
                        $"The discount exceeds the unit price for cart item {item.Id}.");
                }

                orderItems.Add(new OrderItem
                {
                    ProductVariantId = item.ProductVariantId,

                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    UnitDiscount = unitDiscount,
                    LineTotal = effectiveUnitPrice * item.Quantity,
                    CreatedAt = createdAt
                });
            }
            return orderItems;
        }

        private static OrderAddress CreateShippingAddress(PlaceOrderDTO dto, DateTime createdAt)
        {
            return new OrderAddress
            {
                FullName = $"{dto.FirstName} {dto.LastName}",
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                StateOrRegion = dto.StateOrRegion,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = createdAt
            };
        }

        private static OrderAddress CreateBillingAddress(PlaceOrderDTO dto, DateTime createdAt)
        {
            return new OrderAddress
            {
                FullName = $"{dto.FirstName} {dto.LastName}",
                AddressLine1 = dto.BillingAddressLine1,
                AddressLine2=dto.BillingAddressLine2,

                City = dto.BillingCity,
                Country = dto.BillingCountry,
                PostalCode = dto.BillingPostalCode,
                StateOrRegion = dto.BillingStateOrRegion,

                PhoneNumber = dto.PhoneNumber,
                CreatedAt = createdAt
            };
        }


        private static DateTime AddBusinessDays(DateTime date, int businessDays)
        {
            if (businessDays < 0)
                throw new ArgumentOutOfRangeException(nameof(businessDays));

            var result = date;
            var addedDays = 0;

            while (addedDays < businessDays)
            {
                result = result.AddDays(1);

                if (result.DayOfWeek is not DayOfWeek.Saturday
                    and not DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }

            return result;
        }

        public async Task <List<OrderDTO>> GetAllUserOrderDTOSAsync(string userId)
        {
                var orderDTOs = new List<OrderDTO>();

                var userOrders = await _orderRepo.GetAllByUserIdAsync(userId);
               
                foreach (var order in userOrders)
                {

                    orderDTOs.Add(MapToOrderDTO(order));
                }

                return orderDTOs;
        
        }

      

        private List<OrderItemDTO> MapToOrderItemDTO(List<OrderItem> orderItems)
        {
            var orderItemDTOs = new List<OrderItemDTO>();
            foreach (var item in orderItems)
            {
                if(item.ProductVariant==null)
                {
                    throw new InvalidOperationException("productVariant must be defined for the orderitem");
                }

                var firstImage = item.ProductVariant.ProductImages.FirstOrDefault();

                if (firstImage is null)
                {
                    throw new InvalidOperationException(
                        "ProductVariant must have at least one image.");
                }
                orderItemDTOs.Add(new OrderItemDTO()
                {
                    Quantity = item.Quantity,
                    Product = new ProductCardDTO()
                    {
                        Id = item.ProductVariant.ProductId,
                        VariantId = item.ProductVariant.Id,
                        ImageUrl = _objectStorage.GetPublicUrl(firstImage.StorageKey),
                        Keywords = item.ProductVariant.Product.ProductTags.ToList().Select(pt => pt.Tag.Name).ToList(),
                        Name = item.ProductVariant.Product.Name,
                        Rating = new RatingDTO() { Count = item.ProductVariant.Product.RatingCount, AverageRating = item.ProductVariant.Product.AverageRating },
                        Price = item.ProductVariant.Price,
                    },
                    ProductId = item.ProductVariant.ProductId,

                });
            }

            return orderItemDTOs;

        }

        public OrderDTO MapToOrderDTO(Order order)
        {
            return new OrderDTO()
            {
                Id = order.Id,
                OrderItems = MapToOrderItemDTO(order.OrderItems.ToList()),
                TotalCost = order.Total,
                CreatedAt = order.CreatedAt,
                EstimatedDeliveryFrom = order.EstimatedDeliveryFrom,
                EstimatedDeliveryTo = order.EstimatedDeliveryTo,
                Email = order.Customer.Email,
                Status = order.Status

            };
        }

     

        public async Task<Order> GetOrderWithOrderNumber(string orderNumber)
        {
            var order=await _orderRepo.GetByOrderNumber(orderNumber);

            if (order == null)
            {
                throw new KeyNotFoundException("The order could not be found.");
            }

           

            return order;

        }

        public async Task<OrderDTO> GetUserOrderDTOAsync(int orderId, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException();
            }

           

            var userOrder = await _orderRepo.GetByUserId(orderId,userId);
            
            if(userOrder==null)
            {
                throw new KeyNotFoundException("Could not find an order matching the orderid and userid that was provided");
            }

            return MapToOrderDTO(userOrder);
        
        }

      

       

       
    }
}
