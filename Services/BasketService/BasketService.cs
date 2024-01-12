using Arowolo_Delivery_Project.Data;
using Arowolo_Delivery_Project.Dtos.BasketDto;
using Arowolo_Delivery_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Arowolo_Delivery_Project.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public BasketService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddDishToCart(Guid dishId, string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                throw new Exception("No active user");
            }

            var dish = await _context.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);

            if (dish == null)
            {
                throw new Exception("Dish with id doesn't exist");
            }

            var dishInCart = await _context.Baskets.FirstOrDefaultAsync(dishInCart => dishInCart.Dish.Id == dishId && dishInCart.User.Id == currentUser.Id && dishInCart.Order.Id == null);

            if (dishInCart != null)
            {
                dishInCart.Count += 1;
            }
            else
            {
                var addToCart = new Basket
                {
                    Dish = dish,
                    Count = 1,
                    User = currentUser,
                };
                _context.Baskets.Add(addToCart);
            }

            await _context.SaveChangesAsync();
        }


        public async Task DeleteDishInCart(Guid dishId, bool increase, string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser == null)
            {
                throw new Exception("No active user");
            }

            var dish = await _context.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);

            if (dish == null)
            {
                throw new Exception("Dish doesn't exist");
            }

            var dishInCart = await _context.Baskets.FirstOrDefaultAsync(dishInCart => dishInCart.Dish.Id == dishId && dishInCart.User.Id == currentUser.Id && dishInCart.Order.Id == null);

            if (dishInCart != null)
            {
                if (increase == true)
                {
                    dishInCart.Count -= 1;
                }

                else
                {
                    _context.Baskets.Remove(dishInCart);
                }
            }
            await _context.SaveChangesAsync();
        }



        public async Task<List<DishBasketDto>> GetBasket(string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser == null)
            {

                return new List<DishBasketDto>();
            }

            var dishInCartList = await _context.Baskets.Where(basket => basket.User.Id == currentUser.Id && basket.Order.Id == null).Include(basket => basket.Dish).ToListAsync();


            var cartList = dishInCartList.Select(basket => new DishBasketDto
            {
                Name = basket.Dish.Name,

                Price = basket.Dish.Price,

                TotalPrice = basket.Dish.Price * basket.Count,

                Amount = basket.Count,

                Image = basket.Dish.PhotoUrl,
            }).ToList();

            return cartList;
        }
    }
}
