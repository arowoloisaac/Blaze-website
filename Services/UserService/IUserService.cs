using Arowolo_Delivery_Project.Dtos.UserDtos;
using Arowolo_Delivery_Project.Models;

namespace Arowolo_Delivery_Project.Services.UserService
{
    public interface IUserService
    {
        Task<tokenResponse> Register(RegisterUserDto request);

        Task<UserProfileDto> GetProfile(string email);
        
        Task<tokenResponse> Login(LoginUserDto request);

        //Task<UserProfileDto> EditProfile(EditUserDto request);

        Task EditProfile(EditUserDto request, string Id);
    }
}
