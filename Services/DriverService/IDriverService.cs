using startup_trial.Dtos.DriverDtos;
using startup_trial.Models;

namespace startup_trial.Services.DriverService
{
    public interface IDriverService
    {
        Task EditProfile(EditDriverDto request, string Id);

        Task<GetDriverProfile> GetProfile(string email);

        Task<tokenResponse> Login(LoginDriverDto request);

        Task<tokenResponse> Register(RegisterDriverDto request);
    }
}
