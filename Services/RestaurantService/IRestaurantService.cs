using startup_trial.Dtos.RestaurantDto;

namespace startup_trial.Services.RestaurantService
{
    public interface IRestaurantService
    {
        Task RegisterRestaurant(RegisterRestaurantDto model);

        Task<string> Login (LoginRestaurantDto model);

        Task EditRestaurantProfile(EditRestaurantProfile model, string email);

        Task<RestaurantProfile> GetProfile(string email);
    }
}
