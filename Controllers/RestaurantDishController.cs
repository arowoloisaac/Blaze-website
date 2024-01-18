using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using startup_trial.Dtos.RestaurantDto.RestaurantDishDto;
using startup_trial.Services.RestaurantDish;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace startup_trial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantDishController : ControllerBase
    {
        private readonly IRestaurantDish _restaurant;

        public RestaurantDishController(IRestaurantDish restaurantDish)
        {
            _restaurant = restaurantDish;
        }

        [HttpPost("dish")]
        [Authorize]
        [SwaggerOperation(Summary ="Add dish to your restaurant")]
        public async Task<IActionResult> AddDish(AddRestaurantDishDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userClaimById = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
            await _restaurant.AddDishes(model, userClaimById.Value);

            return Ok();
        }

        [HttpGet("dish")]
        [Authorize]
        [SwaggerOperation(Summary ="Restaurant to get its dish list")]
        public async Task<IActionResult> FetchDishes()
        {
            var userClaimById = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

            return Ok(await _restaurant.FetchDish(userClaimById.Value));
        }

        [HttpGet("restaurant-portfolio")]
        [Authorize]
        public async Task<IActionResult> RestaurantPortfolio()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);

            return Ok(await _restaurant.Portfolio(userClaim.Value));
        }
    }
}
