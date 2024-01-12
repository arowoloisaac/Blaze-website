using Arowolo_Delivery_Project.Cofiguration;
using Arowolo_Delivery_Project.Dtos.UserDtos;
using Arowolo_Delivery_Project.Models;
using Arowolo_Delivery_Project.Services.TokenService;
using Arowolo_Delivery_Project.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Arowolo_Delivery_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private ITokenStorageService _tokenStorageService;

        public UserController(IUserService userService, ITokenStorageService tokenStorageService)
        {
            _userService = userService;
            _tokenStorageService = tokenStorageService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.Register(user);
                return Ok();
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile() 
        {  
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                return await _userService.GetProfile(emailClaim.Value);
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
        public async Task<IActionResult> EditProfile(EditUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {                
                var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
                await _userService.EditProfile(model, userIdClaim.Value);
                return Ok();
            }

            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            try
            {
                return Ok(await _userService.Login(model));
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

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var id = Guid.Parse(User.FindFirst(ClaimTypes.Authentication).Value);
            _tokenStorageService.LogoutToken(id);
            return Ok();
        }
    }
}
