using Arowolo_Delivery_Project.Dtos.BasketDto;
using Arowolo_Delivery_Project.Dtos.DishDto;
using Arowolo_Delivery_Project.Dtos.OrderDtos;
using Arowolo_Delivery_Project.Models;
using AutoMapper;

namespace Arowolo_Delivery_Project.Cofiguration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Dish, GetDishDto>();
            CreateMap<AddDishDto, Dish>();
            CreateMap<RatingDto, Rating>();
            CreateMap<Rating,  RatingDto>();
            CreateMap<Order, GetOrderInfoDto>();
            CreateMap<Order, GetOrderDto>();
            CreateMap<Basket, DishBasketDto>();
        }
    }
}


/**/