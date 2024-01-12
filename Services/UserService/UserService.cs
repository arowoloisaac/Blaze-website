using Arowolo_Delivery_Project.Cofiguration;
using Arowolo_Delivery_Project.Dtos.UserDtos;
using Arowolo_Delivery_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Arowolo_Delivery_Project.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtBearerTokenSettings _bearerTokenSettings;

        public UserService(UserManager<User> userManger, IOptions<JwtBearerTokenSettings> jwtTokenOptions)
        {
            _userManager = userManger;
            _bearerTokenSettings = jwtTokenOptions.Value;
        }

        public async Task EditProfile(EditUserDto request, string Id)
        {

            var currentUser = await _userManager.FindByIdAsync(Id);

            if (currentUser is null)
            {
                throw new ArgumentNullException("No Active user");
            }

            currentUser.Address = request.Address;
            currentUser.BirthDate = request.BirthDate;
            currentUser.PhoneNumber = request.PhoneNumber;
            currentUser.Gender = request.Gender;
            currentUser.FullName = request.FullName;

            var updateUser = await _userManager.UpdateAsync(currentUser);

            if (!updateUser.Succeeded)
            {
                throw new Exception("Failed to update user profile");
            }
            
        }

        public async Task<UserProfileDto> GetProfile(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} already exist");
            }

            return new UserProfileDto
            {
                Email = user.Email,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                Name = user.FullName,
                Address = user.Address,
                Gender = user.Gender,
                Id = user.Id,
            };
        }

        public async Task<tokenResponse> Login(LoginUserDto request)
        {
            var user = await ValidateUser(request);

            if (user == null)
            {
                throw new InvalidOperationException("Login Failed");
            }

            var role = await _userManager.IsInRoleAsync(user, ApplicationRoleNames.User);
            var token = GenerateToken(user);

            return new tokenResponse(token);
        }

        public async Task<tokenResponse> Register(RegisterUserDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ArgumentNullException("User with same Email Exists");
            }

            var identityUser = new User
            {
                UserName = request.Email,
                FullName = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthdDate,
                Address = request.Address,
                Gender = request.Gender,
            };

            var saveUser = await _userManager.CreateAsync(identityUser, request.Password);

            if (!saveUser.Succeeded)
            {
                throw new Exception($"Failed to validate user {request.Email}");
            }

            var token = GenerateToken(identityUser);

            return new tokenResponse(token);
        }



        private async Task<User> ValidateUser(LoginUserDto request)
        {
            var identifyUser = await _userManager.FindByEmailAsync(request.Email);

            if (identifyUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identifyUser, identifyUser.PasswordHash, request.Password);

                return result == PasswordVerificationResult.Success ? identifyUser : null;
            }
            return null;
        }


        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_bearerTokenSettings.SecretKey);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Authentication, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(_bearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _bearerTokenSettings.Audience,
                Issuer = _bearerTokenSettings.Issuer,
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
