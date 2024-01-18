using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using startup_trial.Dtos.RestaurantDto;
using startup_trial.Services.RestaurantService;
using System.Security.Claims;

namespace startup_trial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterRestaurant(RegisterRestaurantDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _restaurantService.RegisterRestaurant(model);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(" Invalid");
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginRestaurant([FromBody] LoginRestaurantDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _restaurantService.Login(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw new Exception("User invalid");
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetRestaurant()
        {
            var userClaim = User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Email);

            var user = await _restaurantService.GetProfile(userClaim.Value);
            return Ok(user);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditRestaurantProfile model)
        {
            var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            await _restaurantService.EditRestaurantProfile(model, emailClaim.Value);

            return Ok();
        }
    }
}
