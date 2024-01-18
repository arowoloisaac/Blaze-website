using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using startup_trial.Data;
using startup_trial.Dtos.DeliveryDto;
using startup_trial.Dtos.DishDto;
using startup_trial.Dtos.RestaurantDto.RestaurantDishDto;
using startup_trial.Enums;
using startup_trial.Models;

namespace startup_trial.Services.RestaurantDish
{
    public class RestaurantDish : IRestaurantDish, IUserRestaurantDish
    {
        private readonly UserManager<User> _manager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RestaurantDish(IMapper mapper, UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            _manager = userManager;
            _mapper = mapper;
            _context = dbContext;

        }

        public async Task AddDishes(AddRestaurantDishDto newDish, string RestaurantId)
        {
            var user = await _manager.FindByIdAsync(RestaurantId);
            

            if (user is Restaurant restaurant)
            {
                var dish = await _context.Dishes.FirstOrDefaultAsync(dish => dish.Name.ToLower() == newDish.Name.ToLower() && dish.RestaurantId == restaurant.Id);

                if (dish is not null)
                {
                    throw new ArgumentException($"Dish with {dish.Name} already exist");
                }

                var DishToAdd = _mapper.Map<Dish>(newDish);
                DishToAdd.RestaurantId = restaurant.Id;
                
                //dish.RestaurantId == restaurant.Id;
                _context.Dishes.Add(DishToAdd);
                await _context.SaveChangesAsync();

                var dbDishes = await _context.Dishes.ToListAsync();

                var mapperDishes = dbDishes.Select(dish => _mapper.Map<GetDishDto>(dish)).ToList();
            }
            
            else
            {
                throw new Exception("You don't have the aurothization to add");
            }

            //return mapperDishes;
        }

        public async Task<List<GetDishDto>> FetchDish(string UserId)
        {
            var user = await _manager.FindByIdAsync(UserId);
            var dish = _context.Dishes.Where(dish => dish.RestaurantId == user.Id).ToList();

            var mappedDish = dish.Select( dish => _mapper.Map<GetDishDto>(dish)).ToList();
            return mappedDish;
        }

        public async Task<List<GetDishDto>> FetchRestaurantDishes(Guid RestaurantId)
        {
            var dishes = _context.Dishes.Where(dish => dish.RestaurantId == RestaurantId).ToList();

            if ( dishes == null)
            {
                throw new ArgumentNullException("resturant have no dish");
            }

            if (dishes.Count < 1)
            {
                return new List<GetDishDto>();
            }

            var mappedDish = dishes.Select(dish => _mapper.Map<GetDishDto>(dish)).ToList();
            return mappedDish;
        }

        public async Task<List<RestaurantListDto>> GetRestaurant()
        {
            var restaurants =  await _manager.Users.OfType<Restaurant>().ToListAsync();


            var getRestaurant = restaurants.Select(r => new RestaurantListDto
            {
                Id = r.Id,
                Name = r.RestaurantName,
            }).ToList();

            return getRestaurant;
        }

        public async Task<List<RestaurantOrderDto>> Portfolio(string UserId)
        {
            var deliverer = await _manager.FindByIdAsync(UserId);

            //var portFolio = await _context.Order.Where(order => order.Status == Status.Delivered && order.DriverId == deliverer.Id).ToListAsync();
            if (deliverer is Restaurant restaurant)
            {
                var portfolio = await _context.Order.Where(order => order.RestaurantId == deliverer.Id && order.Status == Status.Delivered).ToListAsync();

                var lst = portfolio.Select(folio => new RestaurantOrderDto
                {
                    //name of deliverer
                    Name = deliverer.FullName,
                    Earned = portfolio.Sum(order => order.Price * portfolio.Count),
                    Delivered = portfolio.Count

                }).ToList();

                return lst;
            }

            else
            {
                throw new Exception("Not authorized for this task");
            }
        }
    }
}
