using Business.IServices;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        public IPaymentServices _paymentServices;
        public PaymentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        [Authorize]
        [HttpPost("CheckoutCart")]
        public async Task<IActionResult> CheckoutCart(Request_CheckoutCart request_Checkout)
        {
            var result = await _paymentServices.CheckoutCart(request_Checkout);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("Insert_ShippingAddress")]
        public async Task<IActionResult> Insert_ShippingAddress(Request_ShippingAddress request)
        {
            var userID= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _paymentServices.Insert_ShippingAddress(request, userID);
            return Ok();
        }
        [Authorize]
        [HttpGet("Get_ShippingAddress")]
        public async Task<IActionResult> Get_ShippingAddress()
        {
            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _paymentServices.Get_ShippingAddress(userID);
            return Ok(result);
        }

    }
}
