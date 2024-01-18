using startup_trial.Data;
using startup_trial.Dtos.BasketDto;
using startup_trial.Dtos.OrderDtos;
using startup_trial.Enums;
using startup_trial.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace startup_trial.Services.OrderService
{
    public class OrderService : IOrderService
    {

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(UserManager<User> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }


        public async Task<ResponseString> ConfirmOrder(Guid OrderId, string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser == null)
            {
                throw new Exception("User is not active");
            }

            if (currentUser is Customer)
            {
                var order = await _context.Order.FirstOrDefaultAsync(order => order.Id == OrderId);

                if (order == null)
                {
                    throw new ArgumentNullException("Order can't be found");
                }

                order.Status = Status.Delivered;

                await _context.SaveChangesAsync();

                return new ResponseString("Confirmed");
            }

            else
            {
                return new ResponseString("Unable to confirm order, because you are not unauthorize for this action!");
            }
        }

        public async Task<List<GetOrderInfoDto>> GetOrder(string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser is null)
            {
                throw new Exception("User is not active");
            }

            if (currentUser is Customer)
            {
                var getOrder = await _context.Order.Where(order => order.UserId == currentUser.Id).ToListAsync(); //.ToListAsync() //.Include(basket => basket.Baskets).ToListAsync();

                /*var getOrderInfo = new GetOrderInfoDto
                {
                    Id = currentUser.Id,
                    DeliveryTime = getOrder.DeliveryTime,
                    OrderTime = getOrder.OrderTime,
                    Status = getOrder.Status,
                    Price = getOrder.Price,
                };*/

                var getOrderInfo = _mapper.Map<List<GetOrderInfoDto>>(getOrder);
                return getOrderInfo;
            }
            
            else
            {
                throw new Exception("Unauthorized to perform this action!");
            }
        }

        public async Task<GetOrderDto> GetOrderById(Guid OrderId, string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser is null)
            {
                throw new Exception("User is not active");
            }

            if (currentUser is Customer customer)
            {
                var order = await _context.Order.Include(order => order.Baskets).ThenInclude(order => order.Dish).FirstOrDefaultAsync(order => order.Id == OrderId && order.UserId == currentUser.Id);

                if (order is null)
                {
                    throw new Exception("Order doesn't exist");
                }

                var getOrder = _mapper.Map<GetOrderDto>(order);

                var cartList = order.Baskets.Select(basket => new DishBasketDto
                {
                    Name = basket.Dish.Name,
                    Price = basket.Dish.Price,
                    TotalPrice = basket.Dish.Price * basket.Count,
                    Amount = basket.Count,
                    Image = basket.Dish.PhotoUrl
                }).ToList();

                getOrder.Dishes = cartList;
                return getOrder;
            }

            else
            {
                throw new Exception("Unauthorized to perform this action!");
            }
              
        }

        public async Task<ResponseString> PostOrder(CreateOrderDto model, string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser == null)
            {
                throw new Exception("User no found");
            }

            if (currentUser is Customer customer)
            {
                var userBasket = await _context.Baskets
                .Include(basket => basket.Dish)
                .Where(basket => basket.User.Id == currentUser.Id && basket.Order.Id == null)
                .ToListAsync();

                if (userBasket.Count < 1)
                {
                    throw new Exception("No dish in the cart");
                }
                var totalPrice = userBasket.Sum(item => item.Dish.Price * item.Count);

                var newOrder = new Order
                {
                    DeliveryTime = model.DeliveryTime,
                    OrderTime = DateTime.UtcNow,
                    Address = model.Address,
                    UserId = currentUser.Id,
                    Status = Status.InProcess,
                    Price = totalPrice,
                    RestaurantId = userBasket[0].RestaurantId,
                };


                foreach (var item in userBasket)
                {
                    item.Order = newOrder;
                    item.Order.Id = newOrder.Id;
                }

                _context.Order.Add(newOrder);
                // probably add the orderid to the basket to update it 
                await _context.SaveChangesAsync();

                var response = "Successful";
               
                return new ResponseString(response);
            }
            else
            {
                return new ResponseString("Unsuccessful");
            }
        }
    }
}
