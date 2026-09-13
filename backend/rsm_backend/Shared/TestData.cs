using rsm_backend.Application.DTO;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class TestData
    {
        public static Order CreateValidOrder()
        {
            var image = new ProductImage
            {
                StorageKey = "images/test.jpg"
            };

            var product = new Product
            {
                Name = "Test Product",
                RatingCount = 10,
                AverageRating = 4.5m,
                ProductTags = new List<ProductTag>()
            };

            var variant = new ProductVariant
            {
                Id = 5,
                ProductId = 1,
                Price = 99.99m,
                Product = product,
                ProductImages = new List<ProductImage>
                {
                    image
                }
            };

            return new Order
            {
                Id = 123,
                Total = 199.98m,
                CreatedAt = new DateTime(2026, 8, 1),
                EstimatedDeliveryFrom = new DateTime(2026, 8, 5),
                EstimatedDeliveryTo = new DateTime(2026, 8, 10),

                Customer = new Customer
                {
                    Email = "test@test.com"
                },

                OrderItems = new List<OrderItem>
                {
                    new OrderItem {
                        Quantity = 2,
                        ProductVariant = variant
                    }
                }
            };
        }

        public static Product CreateValidProduct()
        {
            return new Product()
            {
                Id = 1,
                Name = "t-shirt",
                ProductTags = new List<ProductTag>() { new ProductTag() { Tag = new Tag() { Name = "T-shirt" } } },
                ProductCategories = new List<ProductCategory>() { new ProductCategory() { Category = new Category() { Name = "clothing" } } },
                Brand = new Brand() { Name = "Levi" },

                CreatedAt = DateTime.UtcNow


            };
        }

        public static ProductVariant CreateValidProductVariant()
        {
            return new ProductVariant()
            {
                ProductId = 1,
                Product = CreateValidProduct(),
                Sku = "TSHRT-BLK-XL",
                Price = 99.99M,
                ProductImages = new List<ProductImage>() { new ProductImage() { StorageKey = "images/1/123/1" } },

                CreatedAt = DateTime.UtcNow




            };
        }

        public static CartItem CreateValidCartItem()
        {
            return new CartItem()
            {
                ProductVariantId = 2,
                Name = "t-shirt-Black",
                UnitPrice = 99.99m,
                ProductVariant = CreateValidProductVariant(),
                Quantity = 2,


            };

        }

        public static Cart CreateValidGuestCart()
        {
            return new Cart()
            {
                Id = 1,
                Customer = null,
                Status = CartStatus.Active,
                Items = new List<CartItem>() { CreateValidCartItem() },
                DeliveryOptionId = 1,
                DeliveryOption = new DeliveryOption()
                {
                    Id = 1,
                    MinDeliveryDays = 2,
                    MaxDeliveryDays = 4,
                    Name = "DAO",
                    Price = 49.99m
                },



            };

        }

        public static PlaceOrderDTO CreateValidPlaceOrderDTO()
        {
            return new PlaceOrderDTO()
            {
                FirstName = "Henrik",
                LastName = "Hansen",
                Email = "customer@email.dk",
                AddressLine1 = "CustomerRoad 54",
                AddressLine2 = "",
                City = "Brabrand",
                StateOrRegion = "",
                PostalCode = "8220",
                Country = "Denmark",
                PhoneNumber = "12345678",
                CouponCode = "coupon1",

                BillingSameAsShipping = true,


            };

        }






    }
}
