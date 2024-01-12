using Arowolo_Delivery_Project.Dtos.DishDto;
using Arowolo_Delivery_Project.Enums;
using Arowolo_Delivery_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arowolo_Delivery_Project.Services.DishService
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
