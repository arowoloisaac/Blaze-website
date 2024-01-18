using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using startup_trial.Cofiguration;
using startup_trial.Dtos.DriverDtos;
using startup_trial.Dtos.UserDtos;
using startup_trial.Models;
using startup_trial.Services.CustomerService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace startup_trial.Services.DriverService
{
    public class DriverService : IDriverService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtBearerTokenSettings _bearerTokenSettings;

        public DriverService(UserManager<User> userManger, IOptions<JwtBearerTokenSettings> jwtTokenOptions)
        {
            _userManager = userManger;
            _bearerTokenSettings = jwtTokenOptions.Value;
        }

        public async Task EditProfile(EditDriverDto request, string Id)
        {

            var currentUser = await _userManager.FindByIdAsync(Id);

            if (currentUser is null)
            {
                throw new ArgumentNullException("No Active user");
            }
            if (currentUser is Driver driver)
            {
                driver.Address = request.Address;
                driver.PhoneNumber = request.PhoneNumber;
                driver.FullName = request.FullName;
                driver.PlateNumber = request.PlateNumber;
                driver.CarType = request.CarType;
                driver.CarColor = request.CarColor;
            }

            var updateUser = await _userManager.UpdateAsync(currentUser);

            if (!updateUser.Succeeded)
            {
                throw new Exception("Failed to update user profile");
            }

        }

        public async Task<GetDriverProfile> GetProfile(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} already exist");
            }

            if (user is Driver driver)
            {
                return new GetDriverProfile
                {
                    Email = driver.Email,
                    BirthDate = driver.BirthDate,
                    PhoneNumber = driver.PhoneNumber,
                    Name = driver.FullName,
                    Gender = driver.Gender,
                    PlateNumber = driver.PlateNumber, 
                    CarType = driver.CarType,
                    CarColor = driver.CarColor,
                    Id = user.Id,
                };
            }

            else
            {
                return null;
            }

        }

        public async Task<tokenResponse> Login(LoginDriverDto request)
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

        public async Task<tokenResponse> Register(RegisterDriverDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ArgumentNullException("User with same Email Exists");
            }


            var identityUser = new Driver
            {
                UserName = request.Email,
                FullName = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthdDate,
                Address = request.Address,
                Gender = request.Gender,
                CarColor= request.CarColor,
                CarType = request.CarType,
                PlateNumber = request.PlateNumber,
            };

            var saveUser = await _userManager.CreateAsync(identityUser, request.Password);

            if (!saveUser.Succeeded)
            {
                throw new Exception($"Failed to validate user {request.Email}");
            }

            var token = GenerateToken(identityUser);

            return new tokenResponse(token);
        }



        private async Task<User> ValidateUser(LoginDriverDto request)
        {
            var identifyUser = await _userManager.FindByEmailAsync(request.Email);

            if (identifyUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identifyUser, identifyUser.PasswordHash, request.Password);

                if (identifyUser is Driver driver)
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
