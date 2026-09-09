using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Domain.Entities;
using System.Security.Claims;
using System.Security.Cryptography;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController:ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        private readonly IGuestOrderAccessService _guestOrderAccessService;
        private readonly IGuestOrderTokenService _guestOrderTokenService;

        public OrderController (IOrderService orderService, IGuestOrderAccessService guestOrderAccessService, IEmailService emailService, IGuestOrderTokenService guestOrderTokenService )
        {
            _orderService = orderService;
            _guestOrderAccessService= guestOrderAccessService;
            _emailService= emailService;
            _guestOrderTokenService= guestOrderTokenService;
        }


        [HttpPost("guest/request-access")]
        [AllowAnonymous]
        public async Task<ActionResult> RequestGuestAccess([FromBody] GuestAccessDTO guestAccessDTO,CancellationToken cancellationToken)
        {
            await _guestOrderAccessService.RequestAccessAsync(guestAccessDTO.OrderNumber, guestAccessDTO.Email,cancellationToken);
            
            return Ok("If the order details are valid, a verification code has been sent.");

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<PlaceOrderResponseDTO>> AddCartToOrder([FromBody] PlaceOrderDTO dto, CancellationToken cancellationToken=default)
        {
            var result=await _orderService.AddCartToOrderAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetOrder), new {orderId=result.OrderId},result);


        }


        [HttpPost("guest/verify")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyGuestAccess([FromBody] GuestVerifyDTO guestVerifyDTO, CancellationToken cancellationToken)
        {
            var result = await _guestOrderAccessService.VerifyGuestAsync(guestVerifyDTO.OrderNumber, guestVerifyDTO.Code, cancellationToken);
            
            return result switch
            {
                GuestVerificationResult.Success =>
                    Ok(new
                    {
                        orderNumber=guestVerifyDTO.OrderNumber,
                        guestToken = _guestOrderTokenService.GenerateToken(guestVerifyDTO.OrderNumber)
                    }),

                GuestVerificationResult.TooManyAttempts =>
                    BadRequest(new { message = "Unable to verify this order." }),

                GuestVerificationResult.Expired =>
                    BadRequest(new { message = "Unable to verify this order." }),

                GuestVerificationResult.InvalidCode =>
                    BadRequest(new { message = "Unable to verify this order." }),

                GuestVerificationResult.NotFound =>
                    BadRequest(new { message = "Unable to verify this order." }),

                _ => StatusCode(500)
            };
        }

        [Authorize(AuthenticationSchemes ="GuestOrder")]
        [HttpGet("guest/{orderNumber}")]
        public async Task<ActionResult<OrderDTO>> GetGuestOrder(string orderNumber)
        {
            var tokenOrderNumber = User.FindFirst("orderNumber")?.Value;

            var purpose = User.FindFirst("purpose")?.Value;

            if (purpose != "guest-order-access")
                return Forbid();

            if (tokenOrderNumber != orderNumber)
                return Forbid();

            var order = await _orderService.GetOrderWithOrderNumber(orderNumber);

            return order is null
                ? NotFound()
                : Ok(_orderService.MapToOrderDTO(order));

        }



        [Authorize]
        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int orderId) 
        {
            var userId = User.Identity?.IsAuthenticated == true
                   ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                   : null;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                   "You must be a logged in user");
              
            }

            var order = await _orderService.GetUserOrderDTOAsync(orderId, userId);

            return Ok(order);

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllUserOrders()
        {


            if (User.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException(
                    "You must be logged in to access your orders.");
            }

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "The authenticated user ID is missing.");
            }

            var orders = await _orderService
                .GetAllUserOrderDTOSAsync(userId);

            return Ok(orders);
        }

    }
}
