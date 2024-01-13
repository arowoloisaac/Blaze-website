using startup_trial.Dtos.BasketDto;
using startup_trial.Models;

namespace startup_trial.Services.BasketService
{
    public interface IBasketService
    {
        Task AddDishToCart(Guid dishId, string userId);

        Task DeleteDishInCart(Guid dishId, bool increase, string userId);

        Task<List<DishBasketDto>> GetBasket(string userId);
    }
}
