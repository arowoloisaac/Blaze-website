using Arowolo_Delivery_Project.Dtos.BasketDto;
using Arowolo_Delivery_Project.Models;

namespace Arowolo_Delivery_Project.Services.BasketService
{
    public interface IBasketService
    {
        Task AddDishToCart(Guid dishId, string userId);

        Task DeleteDishInCart(Guid dishId, bool increase, string userId);

        Task<List<DishBasketDto>> GetBasket(string userId);
    }
}
