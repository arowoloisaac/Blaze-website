using startup_trial.Dtos.DishDto;
using startup_trial.Dtos.RestaurantDto.RestaurantDishDto;

namespace startup_trial.Services.RestaurantDish
{
    public interface IRestaurantDish
    {
        Task AddDishes(AddRestaurantDishDto newDish, string RestaurantId);
        Task<List<GetDishDto>> FetchDish(string UserId);
    }
}
