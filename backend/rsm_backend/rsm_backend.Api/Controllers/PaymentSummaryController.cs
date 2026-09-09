using Microsoft.AspNetCore.Mvc;
using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;

namespace rsm_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentSummaryController:ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IPaymentSummaryService _paymentSummaryService;

        public PaymentSummaryController(ICartService cartService, IPaymentSummaryService paymentSummaryService)
        {
            _cartService = cartService;
            _paymentSummaryService = paymentSummaryService;
        }

        [HttpGet]
        public async Task<ActionResult<PaymentSummaryDTO>> GetPaymentSummary([FromQuery] string? couponCode)
        {

           var cart= await _cartService.GetOrCreateCurrentCartAsync();

            return Ok(await _paymentSummaryService.GetPaymentSummaryDTOAsync(cart,couponCode));
        }

    }
}
