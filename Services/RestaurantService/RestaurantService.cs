using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using startup_trial.Cofiguration;
using startup_trial.Dtos.RestaurantDto;
using startup_trial.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using Azure.Core;

namespace startup_trial.Services.RestaurantService
{
    public class RestaurantService : IRestaurantService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtBearerTokenSettings _bearerTokenSettings;

        public RestaurantService(UserManager<User> userManager, IOptions<JwtBearerTokenSettings> options)
        {
            _userManager = userManager;
            _bearerTokenSettings = options.Value;
        }

        public async Task EditRestaurantProfile(EditRestaurantProfile request, string email)
        {
            var currentUser = await _userManager.FindByEmailAsync(email);

            if (currentUser is null)
            {
                throw new ArgumentNullException("No Active user");
            }

            if (currentUser is Restaurant restaurant)
            {
                restaurant.Address = request.Address;
                restaurant.Email = request.Email;
                restaurant.PhoneNumber = request.PhoneNumber;
                restaurant.startingTime = request.OpeningTime;
                restaurant.ClosingTime = request.ClosingTime;
                restaurant.RestaurantName = request.Name;
            }

            var updateUser = await _userManager.UpdateAsync(currentUser);

            

            if (!updateUser.Succeeded)
            {
                throw new Exception("Failed to update user profile");
            }
        }

        public async Task<RestaurantProfile> GetProfile(string email)
        {
            var currentUser = await _userManager.FindByNameAsync(email);

            if (currentUser == null)
            {
                throw new InvalidOperationException("No user");
            }

            if (currentUser is Restaurant restaurant)
            {
                return new RestaurantProfile
                {
                    Email = restaurant.Email,
                    Name = restaurant.RestaurantName,
                    Address = restaurant.Address,
                    OpenTime = restaurant.startingTime,
                    CloseTime = restaurant.ClosingTime,
                    PhoneNumber = restaurant.PhoneNumber,
                    Id = restaurant.Id
                };
            }

            else
            {
                return null;
            }
        }

        public async Task<string> Login(LoginRestaurantDto model)
        {
            var checkUser = await ValidateUser(model);

            if (checkUser == null)
            {
                throw new Exception("User with email not found, rather create an account");
            }
            
            var generateToken = GenerateToken(checkUser);
            return generateToken;
        }

        public async Task RegisterRestaurant(RegisterRestaurantDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                throw new Exception("User not found");
            }

            var identity = new Restaurant
            {
                RestaurantName = model.Name,
                Email = model.Email,
                UserName = model.Email,
                Address = model.Address,
                ClosingTime = model.ClosingTime,
                startingTime = model.OpeningTime,
                PhoneNumber = model.PhoneNumber,
            };

            var saveUser = await _userManager.CreateAsync(identity, model.Password);

            if(!saveUser.Succeeded)
            {
                throw new Exception("Unable to save user");
            }
        }



        private async Task<User> ValidateUser(LoginRestaurantDto request)
        {
            var identifyUser = await _userManager.FindByEmailAsync(request.Email);

            if (identifyUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identifyUser, identifyUser.PasswordHash, request.Password);

                if (identifyUser is Restaurant restaurant)
                {
                    return result == PasswordVerificationResult.Success ? identifyUser : null;
                }
                else
                {
                    throw new Exception("This is restauarnt owner sector");
                }
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


