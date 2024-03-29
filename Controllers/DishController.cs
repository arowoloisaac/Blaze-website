﻿using startup_trial.Dtos.DishDto;
using startup_trial.Enums;
using startup_trial.Models;
using startup_trial.Services.DishService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace startup_trial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        public readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<GetDishDto>>> GetDishById(Guid id)
        {
            try
            {
                var dish = await _dishService.GetDishById(id);

                if (dish is not null)
                {
                    return Ok(dish);
                }

                else
                {
                    return NotFound();
                }
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                var response = new Response
                {
                    Status = "Error" + ex.Source,
                    Message = "Internal Service Error: " + ex.Message
                };

                return StatusCode(500, response);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetDishes([FromQuery] List<Category>? category, bool? vegetarian, Sorting? sort, int? page)
        {
            try
            {
                var dishes = await _dishService.GetDishes(category, vegetarian, sort, page);

                if (dishes is not null)
                {
                    return Ok(dishes);
                }

                else { return NotFound("Dish not fount"); }
            }

            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                var response = new Response
                {
                    Status = "Error" + ex.Source,
                    Message = "Internal Service Error: " + ex.Message
                };

                return StatusCode(500, response);
            }

        }


        [HttpGet("{id}/check/rating")]
        [Authorize]
        public async Task<IActionResult> CheckRating(Guid id)
        {
            try
            {
                var UserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                if (UserId == null)
                {
                    return Unauthorized("User is not authorized");
                }
                return Ok(await _dishService.GetDishRating(id, UserId.Value ));
            }

            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                var response = new Response
                {
                    Status = "Error",
                    Message = "Internal Service Error: " + ex.Message
                };

                return StatusCode(500, response);
            }

        }


        [HttpPost("rating")]
        [Authorize]
        //public async Task<IActionResult> PostRating(Guid id, int value)
        [SwaggerOperation(Summary ="Rate a dish")]
        public async Task<IActionResult> PostRating(Guid id, int value)
        {
            try
            {
                var UserId = User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Authentication );
                if (value < 0 || value > 10)
                {
                    throw new Exception("Rating can't be less than Zero nor greater than 10");
                }

                return Ok(await _dishService.AddRating(id, value, UserId.Value));
            }

            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                var response = new Response
                {
                    Status = "Error",
                    Message = "Internal Service Error: " + ex.Message
                };

                return StatusCode(500, response);
            }
        }

        /*[HttpGet("test")]
        public async Task<IActionResult> GetDish([FromQuery] List<Category>? category, bool? vegetarian, Sorting? sort, int? page)
        {
            var result = await _dishService.GetDishes(category, vegetarian, sort, page);
            return Ok(result);        
        }*/

    }
}
