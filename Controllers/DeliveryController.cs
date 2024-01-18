using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using startup_trial.Enums;
using startup_trial.Services.DeliveryService;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace startup_trial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpPost("AcceptOrder")]
        [SwaggerOperation(Summary ="Driver accepts to delivery")]
        public async Task<IActionResult> AcquireOrder(Acquire acquire)
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

            return Ok(await _deliveryService.AcquireOrder(acquire, userClaim.Value));
        }

        [HttpGet("delivery-profile")]
        [SwaggerOperation(Summary ="Driver Check their portfolio")]
        public async Task<IActionResult> GetRestauarantProfile()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

            return Ok(await _deliveryService.DeliveryProfile(userClaim.Value));
        }
    }
}
