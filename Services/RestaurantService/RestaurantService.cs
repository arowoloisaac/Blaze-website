using startup_trial.Dtos.RestaurantDto;

namespace startup_trial.Services.RestaurantService
{
    public class RestaurantService : IRestaurantService
    {
        public async Task<RestaurantProfile> EditRestaurantProfile(EditRestaurantProfile model)
        {
            throw new NotImplementedException();
        }

        public async Task<RestaurantProfile> GetProfile(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(LoginRestaurantDto model)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterRestaurant(RegisterRestaurantDto model)
        {
            throw new NotImplementedException();
        }
    }
}
