using startup_trial.Dtos.BasketDto;
using startup_trial.Dtos.DishDto;
using startup_trial.Dtos.OrderDtos;
using startup_trial.Models;
using AutoMapper;
using startup_trial.Dtos.RestaurantDto.RestaurantDishDto;
using static startup_trial.Services.DishService.DishService;

namespace startup_trial.Cofiguration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Dish, GetDishDto>().ForMember(dest => dest.Rating, opt => opt.MapFrom<AverageRatingResolver>());
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