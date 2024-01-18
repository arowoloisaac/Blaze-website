using startup_trial.Dtos.DishDto;
using startup_trial.Dtos.RestaurantDto.RestaurantDishDto;

namespace startup_trial.Services.RestaurantDish
{
    public interface IUserRestaurantDish
    {
        Task<List<RestaurantListDto>> GetRestaurant();
        Task<List<GetDishDto>> FetchRestaurantDishes(Guid RestaurantId);
    }
}
