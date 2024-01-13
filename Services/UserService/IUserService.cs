using startup_trial.Dtos.UserDtos;
using startup_trial.Models;

namespace startup_trial.Services.UserService
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
