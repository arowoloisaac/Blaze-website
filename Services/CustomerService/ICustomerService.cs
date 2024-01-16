using startup_trial.Dtos.UserDtos;
using startup_trial.Models;

namespace startup_trial.Services.CustomerService
{
    public interface ICustomerService
    {
        Task<tokenResponse> Register(RegisterCustomerDto request);

        Task<CustomerProfileDto> GetProfile(string email);
        
        Task<tokenResponse> Login(LoginCustomerDto request);

        //Task<CustomerProfileDto> EditProfile(EditCustomerDto request);

        Task EditProfile(EditCustomerDto request, string Id);
    }
}
