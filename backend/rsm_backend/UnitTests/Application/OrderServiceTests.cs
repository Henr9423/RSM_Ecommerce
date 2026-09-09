using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using System.Text.RegularExpressions;

namespace UnitTests.Application
{
    public class OrderServiceTests
    {
        private readonly Mock<ICartService> _cartServiceMock = new();
        private readonly Mock<IOrderRepository> _orderRepoMock=new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPaymentSummaryService> _paymentSummaryServiceMock = new();
        private readonly Mock<IEmailService> _emailServiceMock = new();
        private readonly Mock<IObjectStorage> _objectStorageMock = new();

        private readonly OrderService _sut;

        public OrderServiceTests()
        {
            _objectStorageMock
             .Setup(x => x.GetPublicUrl(It.IsAny<string>()))
             .Returns((string path) => $"https://example.com/{path}");

            _sut = new OrderService(
                _cartServiceMock.Object,
                _orderRepoMock.Object,
                _unitOfWorkMock.Object,
                _paymentSummaryServiceMock.Object,
                _emailServiceMock.Object,
                _objectStorageMock.Object
            );

         
        }

        private Order CreateValidOrder()
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

        private OrderDTO CreateExpectedOrderDTO()
        {
                return new OrderDTO
                {
                    Id = 123,
                    TotalCost = 199.98m,
                    CreatedAt = new DateTime(2026, 8, 1),
                    EstimatedDeliveryFrom = new DateTime(2026, 8, 5),
                    EstimatedDeliveryTo = new DateTime(2026, 8, 10),
                    Email = "test@test.com",

                    OrderItems = new List<OrderItemDTO>
                    {
                        new OrderItemDTO
                        {
                            Quantity = 2,
                            ProductId = 1,

                            Product = new ProductCardDTO
                            {
                                Id = 1,
                                VariantId = 5,
                                Name = "Test Product",
                                Price = 99.99m,
                                ImageUrl = "https://example.com/images/test.jpg",

                                Keywords = new List<string>(),

                                Rating = new RatingDTO
                                {
                                    Count = 10,
                                    AverageRating = 4.5m
                                }
                            }
                        }
                    }
                };
        }

        private PlaceOrderDTO CreateValidPlaceOrderDTO()
        {
            return new PlaceOrderDTO()
            {
                FirstName = "Henrik" ,
                LastName = "Hansen",
                Email = "customer@email.dk",
                AddressLine1 = "CustomerRoad 54" ,
                AddressLine2 = "" ,
                City = "Brabrand" ,
                StateOrRegion = "",
                PostalCode = "8220" ,
                Country = "Denmark" ,
                PhoneNumber= "12345678" ,
                CouponCode= "coupon1",

                BillingSameAsShipping= true ,
                

            };

        }

        private Product CreateValidProduct()
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

        private ProductVariant CreateValidProductVariant()
        {
            return new ProductVariant()
            {
                ProductId = 1,
                Product= CreateValidProduct(),
                Sku = "TSHRT-BLK-XL",
                Price = 99.99M,
                ProductImages = new List<ProductImage>() { new ProductImage() { StorageKey = "images/1/123/1" } },
                
                CreatedAt = DateTime.UtcNow

                


            };
        }

        private CartItem CreateValidCartItem()
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

        private Cart CreateValidGuestCart()
        {
            return new Cart()
            {
                Id = 1,
                Customer = null,
                Status = CartStatus.Active,
                Items = new List<CartItem>() { CreateValidCartItem() },
                DeliveryOptionId=1,
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

        private PaymentSummaryDTO CreateExpectedPaymentSummaryDTO()
        {
            return new PaymentSummaryDTO
            {
                ItemsCount = 2,
                ProductCost = 99.99m*2,
                ShippingCost = 49.99m,
                TotalCostBeforeTax = 99.99m * 2+ 49.99m,
                Tax = 0m,
                CouponDiscount = 0,
                TotalCost = 99.99m * 2 + 49.99m
            };
        }


        /// <summary>
        /// GetOrderWithOrderNumber(string orderNumber) 

        [Fact]
        public async Task GetByOrderNumber_WhenOrderExists_ShouldReturnOrder()
        {
            // Arrange
            string orderNumber = "1234";
            DateTime now = DateTime.UtcNow;
            Order expectedOrder = new Order() { CreatedAt=now};

            _orderRepoMock.Setup(i => i.GetByOrderNumber(orderNumber)).ReturnsAsync(expectedOrder);

            // Act

            var result =await _sut.GetOrderWithOrderNumber(orderNumber);

            // Assert

            result.Should().Be(expectedOrder);
        }

        [Fact]
        public async Task GetByOrderNumber_WhenOrderDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            string orderNumber = "1234";
            Order expectedOrder = new Order();

            _orderRepoMock.Setup(i => i.GetByOrderNumber(orderNumber)).ReturnsAsync((Order?)null);

            // Act

            Func<Task> act = () => _sut.GetOrderWithOrderNumber(orderNumber);

            // Assert

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }



        /// <summary>
        /// GetUserOrderDTOAsync(int orderId, string userId)

        [Fact]
        public async Task GetByUserIdAndOrderId_WhenOrderExists_ShouldReturnOrderDTO()
        {
            //Arrange
            int orderId = 1234;
            string userId = "abcd";

            string email = "abc@mail.com";

            DateTime now = DateTime.UtcNow;

            var order = new Order
            {
                Id = orderId,
                Customer = new Customer() { UserId = userId, Email = email },
                Total = 499.95m,
                Status = OrderStatus.Pending,
                CreatedAt = now

            };

            OrderDTO expectedOrderDTO = new OrderDTO()
            {
                Id = order.Id,
                TotalCost = 499.95m,
                Status = OrderStatus.Pending,
                Email=email,
                CreatedAt=now
               
                
            };
           
            _orderRepoMock.Setup(i => i.GetByUserId(orderId, userId)).ReturnsAsync(order);

            // Act

           var result= await _sut.GetUserOrderDTOAsync(orderId, userId);

            // Assert
            result.Should().BeEquivalentTo(expectedOrderDTO);


        }

        [Fact]
        public async Task GetByUserIdAndOrderId_WhenOrderDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            int orderId = 1234;
            string userId = "abcd";

            _orderRepoMock.Setup(i => i.GetByUserId(orderId, userId)).ReturnsAsync((Order?)null);

            // Act 
            Func<Task> act = ()=> _sut.GetUserOrderDTOAsync(orderId, userId);
            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }


        [Fact]
        public async Task GetByUserIdAndOrderId_WhenUserIdIsNullOrWhiteSpace_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            int orderId = 1234;
            string userId = "  ";

            // Act 
            Func<Task> act = () => _sut.GetUserOrderDTOAsync(orderId, userId);
            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }



        // Summary  
        //MapToOrderDTO(Order order)

        [Fact]
        public void MapToOrderDTO_ValidOrder_ReturnsCorrectDTO()
        {
            //Arrange
            Order order = CreateValidOrder();

            _objectStorageMock
                .Setup(x => x.GetPublicUrl("images/test.jpg"))
                .Returns("https://example.com/images/test.jpg");

            OrderDTO expectedOrderDTO = CreateExpectedOrderDTO();

            // Act
            var result = _sut.MapToOrderDTO(order);

            // Assert
            result.Should().BeEquivalentTo(expectedOrderDTO);
   
        }


        [Fact]
        public void MapToOrderDTO_ProductVariantIsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            Order order = CreateValidOrder();
            order.OrderItems.First().ProductVariant = null!;

            //Act
            Func<OrderDTO> act=()=> _sut.MapToOrderDTO(order);

            //Assert
            act.Should().Throw<InvalidOperationException>();

        }

        [Fact]
        public void MapToOrderDTO_ProductVariantHasNoImages_ThrowsInvalidOperationException()
        {
            // Arrange
            Order order = CreateValidOrder();
            order.OrderItems.Single().ProductVariant.ProductImages = new List<ProductImage>();

            //Act
            Func<OrderDTO> act = () => _sut.MapToOrderDTO(order);

            //Assert
            act.Should().Throw<InvalidOperationException>();

        }


        // Summary
        // GetAllUserOrders(string userId)

        [Fact]
        public async Task GetAllUserOrders_WhenOrdersExistsForUser_ReturnOrderDTOs()
        {
            //Arrange
            string userId = "user123";

            _orderRepoMock.Setup(x => x.GetAllByUserIdAsync(userId)).ReturnsAsync(new List<Order>() { CreateValidOrder() });

            List<OrderDTO> expectedOrderDTOs = new List<OrderDTO>() { CreateExpectedOrderDTO() };

            //Act
            var result = await _sut.GetAllUserOrderDTOSAsync(userId);

            //Assert
            result.Should().BeEquivalentTo(expectedOrderDTOs);

            


        }



        //Summary
        // AddCartToOrderAsync(PlaceOrderDTO dto, CancellationToken cancellationToken=default)

        //Tests to do

            //BillingSameAsShipping == true → billing address matches shipping address
            //cart already has a customer → existing customer is used
            //cart has no customer → guest customer is created from the DTO
            //payment summary values are transferred correctly into the order
            //order is added to the repository
            //cart status becomes Checked_Out
            //SaveChangesAsync is called
            //guest customer → confirmation email is sent
            //non - guest customer → confirmation email is not sent
            //returned PlaceOrderResponseDTO contains the correct OrderId and Total

            // Pass order Into repository
                //Order? createdOrder = null;

                //_orderRepoMock
                //    .Setup(x => x.AddOrderAsync(It.IsAny<Order>()))
                //    .Callback<Order>(order => createdOrder = order);

            //Then after ASSERT like this to verify the order was added:
                //createdOrder.Should().NotBeNull();
                //createdOrder!.Subtotal.Should().Be(expectedPaymentSummary.ProductCost);
                //createdOrder.ShippingFee.Should().Be(expectedPaymentSummary.ShippingCost);
                //createdOrder.CouponDiscount.Should().Be(expectedPaymentSummary.CouponDiscount);
                //createdOrder.Total.Should().Be(expectedPaymentSummary.Total);
                //createdOrder.Status.Should().Be(OrderStatus.Pending);

        [Fact]
        public async Task AddCartToOrderAsync_WhenValidCart_ShouldSendOrderConfirmationEmail()
        {
            //Arrange
            PlaceOrderDTO placeOrderDTO = CreateValidPlaceOrderDTO();
            Cart guestCart = CreateValidGuestCart();
            PaymentSummaryDTO expectedPaymentSummary = CreateExpectedPaymentSummaryDTO();

            _cartServiceMock.Setup(x => x.GetOrCreateCurrentCartAsync()).ReturnsAsync(guestCart);

            _paymentSummaryServiceMock.Setup(x => x.GetPaymentSummaryDTOAsync(guestCart, placeOrderDTO.CouponCode)).ReturnsAsync(expectedPaymentSummary);

            //Act
            var result = await _sut.AddCartToOrderAsync(placeOrderDTO);

            //Assert

                _emailServiceMock.Verify(x => x.SendOrderConfirmationAsync(placeOrderDTO.Email, It.IsAny<string>()),Times.Once);
        }

        [Fact]
        public async Task AddCartToOrderAsync_WhenValidCart_ShouldSendAddOrderToDb()
        {

            //Arrange

            Order? createdOrder = null;

            _orderRepoMock
               .Setup(x => x.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => createdOrder = order);

            PlaceOrderDTO placeOrderDTO = CreateValidPlaceOrderDTO();
            Cart guestCart = CreateValidGuestCart();
            PaymentSummaryDTO expectedPaymentSummary = CreateExpectedPaymentSummaryDTO();

            _cartServiceMock.Setup(x => x.GetOrCreateCurrentCartAsync()).ReturnsAsync(guestCart);

            _paymentSummaryServiceMock.Setup(x => x.GetPaymentSummaryDTOAsync(guestCart, placeOrderDTO.CouponCode)).ReturnsAsync(expectedPaymentSummary);

            //Act
            var result = await _sut.AddCartToOrderAsync(placeOrderDTO);

            //Assert

            createdOrder.Should().NotBeNull();
            createdOrder!.Subtotal.Should().Be(expectedPaymentSummary.ProductCost);
            createdOrder.ShippingFee.Should().Be(expectedPaymentSummary.ShippingCost);
            createdOrder.CouponDiscount.Should().Be(expectedPaymentSummary.CouponDiscount);
            createdOrder.Total.Should().Be(expectedPaymentSummary.TotalCost);
            createdOrder.Status.Should().Be(OrderStatus.Pending);
         
        }


        [Fact]
        public async Task AddCartToOrderAsync_WhenPlaceOrderDTOIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            PlaceOrderDTO placeOrderDTO = null;
           
            //Act
            Func<Task> Act= ()=> _sut.AddCartToOrderAsync(placeOrderDTO);

            //Assert

             await Act.Should().ThrowAsync<ArgumentNullException>();
        }



        //cart has no items → throws InvalidOperationException

        [Fact]
        public async Task AddCartToOrderAsync_WhenCartHasNoItems_ThrowsInvalidOperationException()
        {
            //Arrange
            PlaceOrderDTO placeOrderDTO = CreateValidPlaceOrderDTO();
            Cart guestCart = CreateValidGuestCart();
            guestCart.Items = new List<CartItem>();
         
            _cartServiceMock.Setup(x => x.GetOrCreateCurrentCartAsync()).ReturnsAsync(guestCart);


            //Act
            Func<Task> Act = () => _sut.AddCartToOrderAsync(placeOrderDTO);

            //Assert

            await Act.Should().ThrowAsync<InvalidOperationException>();
        }


        //cart has no delivery option → throws InvalidOperationException
        [Fact]
        public async Task AddCartToOrderAsync_WhenCartHasNoDeliveryOption_ThrowsInvalidOperationException()
        {
            //Arrange
            PlaceOrderDTO placeOrderDTO = CreateValidPlaceOrderDTO();
            Cart guestCart = CreateValidGuestCart();
            guestCart.DeliveryOption = null;

            _cartServiceMock.Setup(x => x.GetOrCreateCurrentCartAsync()).ReturnsAsync(guestCart);


            //Act
            Func<Task> Act = () => _sut.AddCartToOrderAsync(placeOrderDTO);

            //Assert

            await Act.Should().ThrowAsync<InvalidOperationException>();
        }


        //BillingSameAsShipping == true → billing address matches shipping address
        [Fact]
        public async Task AddCartToOrderAsync_WhenValidCartWithBillingSameAsShippingIsTrue_ShouldShippingAndBillingAddressMatch()
        {
            //Arrange
            PlaceOrderDTO placeOrderDTO = CreateValidPlaceOrderDTO();
            Cart guestCart = CreateValidGuestCart();
            placeOrderDTO.BillingSameAsShipping = true;
            PaymentSummaryDTO expectedPaymentSummary = CreateExpectedPaymentSummaryDTO();

            Order? createdOrder = null;

            _orderRepoMock
               .Setup(x => x.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => createdOrder = order);

            _cartServiceMock.Setup(x => x.GetOrCreateCurrentCartAsync()).ReturnsAsync(guestCart);

            _paymentSummaryServiceMock.Setup(x => x.GetPaymentSummaryDTOAsync(guestCart, placeOrderDTO.CouponCode)).ReturnsAsync(expectedPaymentSummary);

            //Act
            var result = await _sut.AddCartToOrderAsync(placeOrderDTO);

            //Assert

            createdOrder!.BillingAddress.Should().BeEquivalentTo(
                          createdOrder.ShippingAddress,
                          options => options
                              .Excluding(x => x.Id)
                              .Excluding(x => x.CreatedAt),
                          "the billing address should be the same as the shipping address"
  );
        }




    }
}
