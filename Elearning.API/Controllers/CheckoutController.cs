using Core.Controllers;
using Core.Models;
using Elearning.BL.Interfaces;
using Elearning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CheckoutController
    {
        ICheckoutBL _checkoutBL;

        public CheckoutController(ICheckoutBL checkoutBL)
        {
            _checkoutBL = checkoutBL;
        }

        [HttpPost]
        public async Task<ServiceResponse> Checkout([FromBody] Dictionary<string, object> payload)
        {
            return await _checkoutBL.Checkout(payload);
        }

        [AllowAnonymous]
        [HttpPost("receive-result")]
        public async Task<ServiceResponse> CheckoutResult([FromBody] dynamic param)
        {
            return await _checkoutBL.ReceiveCheckoutResult(param);
        }

        [AllowAnonymous]
        [HttpGet("receive-result")]
        public async Task<ServiceResponse> CheckoutResultV2(string content, int money, string type)
        {
            var param = new Dictionary<string, object>()
            {
                {"content", content },
                {"money", money },
                {"type", type }
            };
            return await _checkoutBL.ReceiveCheckoutResult(param);
        }
    }
}
