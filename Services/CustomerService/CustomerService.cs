using startup_trial.Cofiguration;
using startup_trial.Dtos.UserDtos;
using startup_trial.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace startup_trial.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtBearerTokenSettings _bearerTokenSettings;

        public CustomerService(UserManager<User> userManger, IOptions<JwtBearerTokenSettings> jwtTokenOptions)
        {
            _userManager = userManger;
            _bearerTokenSettings = jwtTokenOptions.Value;
        }

        public async Task EditProfile(EditCustomerDto request, string Id)
        {

            var currentUser = await _userManager.FindByIdAsync(Id);

            if (currentUser is null)
            {
                throw new ArgumentNullException("No Active user");
            }
            if (currentUser is Customer customer)
            {
                customer.Address = request.Address;
                customer.BirthDate = request.BirthDate;
                customer.PhoneNumber = request.PhoneNumber;
                customer.FullName = request.FullName;

            }
            
            var updateUser = await _userManager.UpdateAsync(currentUser);

            if (!updateUser.Succeeded)
            {
                throw new Exception("Failed to update user profile");
            }
            
        }

        public async Task<CustomerProfileDto> GetProfile(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} already exist");
            }

            if (user is Customer customer)
            {
                return new CustomerProfileDto
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

            else
            {
                return null;
            }
            
        }

        public async Task<tokenResponse> Login(LoginCustomerDto request)
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

        public async Task<tokenResponse> Register(RegisterCustomerDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ArgumentNullException("User with same Email Exists");
            }


            var identityUser = new Customer
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



        private async Task<User> ValidateUser(LoginCustomerDto request)
        {
            var identifyUser = await _userManager.FindByEmailAsync(request.Email);

            if (identifyUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identifyUser, identifyUser.PasswordHash, request.Password);

                if (identifyUser is Customer customer)
                {
                    return result == PasswordVerificationResult.Success ? identifyUser : null;
                }

                else
                {
                    throw new Exception("This task is for customer");
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
