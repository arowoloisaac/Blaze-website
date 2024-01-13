using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using startup_trial.Dtos.RestaurantDto;
using startup_trial.Services.RestaurantService;

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
            throw new NotImplementedException();    
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginRestaurant([FromBody] LoginRestaurantDto model)
        {
            throw new NotImplementedException();
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetRestaurant()
        {
            throw new NotImplementedException();
        }

        [HttpPost("profile")]
        public async Task<IActionResult> EditProfile(EditRestaurantProfile model)
        {
            throw new NotImplementedException();
        }
    }
}
