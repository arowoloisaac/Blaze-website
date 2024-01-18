using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using startup_trial.Dtos.DriverDtos;
using startup_trial.Dtos.UserDtos;
using startup_trial.Services.DriverService;
using System.Security.Claims;

namespace startup_trial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverservice;

        public DriverController(IDriverService driverService)
        {
            _driverservice = driverService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDriverDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _driverservice.Register(user);
                return Ok();
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<GetDriverProfile>> GetUserProfile()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                return await _driverservice.GetProfile(emailClaim.Value);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            catch
            {
                return BadRequest();
            }

        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditDriverDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                await _driverservice.EditProfile(model, userIdClaim.Value);
                return Ok();
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDriverDto model)
        {
            try
            {
                return Ok(await _driverservice.Login(model));
            }
            catch (InvalidOperationException ex)
            {
                // Write logs
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return BadRequest();
        }
    }
}
