using startup_trial.Dtos.DishDto;
using startup_trial.Enums;
using startup_trial.Models;
using Microsoft.AspNetCore.Mvc;

namespace startup_trial.Services.DishService
{
    public interface IDishService
    {
        Task<GetDishDto> GetDishById(Guid id);

        Task<List<GetDishDto>> AddDishes(AddDishDto newDish);

        //Task<ServiceResponses> GetDishes(Category? category, bool? vegetarian, Sorting? sort, int? page);

        Task<bool> GetDishRating(Guid dishId, string userId);

        Task<Rating> AddRating(Guid dishId, int value, string UserId);

        Task<ServiceResponses> GetDishes([FromQuery] List<Category>? category, bool? vegetarian, Sorting? sort, int? page);
    }
}
