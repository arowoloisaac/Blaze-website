using startup_trial.Dtos.BasketDto;
using startup_trial.Dtos.DishDto;
using startup_trial.Dtos.OrderDtos;
using startup_trial.Models;
using AutoMapper;
using startup_trial.Dtos.RestaurantDto.RestaurantDishDto;

namespace startup_trial.Cofiguration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Dish, GetDishDto>();
            CreateMap<AddDishDto, Dish>();
            CreateMap<AddRestaurantDishDto, Dish>();
            CreateMap<RatingDto, Rating>();
            CreateMap<Rating,  RatingDto>();
            CreateMap<Order, GetOrderInfoDto>();
            CreateMap<Order, GetOrderDto>();
            CreateMap<Basket, DishBasketDto>();
        }
    }
}


/**/