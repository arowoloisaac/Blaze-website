using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using startup_trial.Services.RestaurantDish;
using Swashbuckle.AspNetCore.Annotations;

namespace startup_trial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRestaurantController : ControllerBase
    {
        private readonly IUserRestaurantDish _restaurantDishes;

        public UserRestaurantController(IUserRestaurantDish userRestaurantDish)
        {
            _restaurantDishes = userRestaurantDish;
        }


        [HttpGet("restaurants")]
        [SwaggerOperation(Summary = "Restaurant List")]
        [Authorize]
        public async Task<IActionResult> FetchRestaurant()
        {
            return Ok(await _restaurantDishes.GetRestaurant());
        }


        [HttpGet("restaurant-dishes")]
        [SwaggerOperation(Summary = "Get dishes of a specific restaurant")]
        [Authorize]
        public async Task<IActionResult> FetchRestaurantDishes(Guid restaurantId)
        {
            return Ok(await _restaurantDishes.FetchRestaurantDishes(restaurantId));
        }
    }
}
