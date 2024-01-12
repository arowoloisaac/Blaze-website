using Arowolo_Delivery_Project.Data;
using Arowolo_Delivery_Project.Dtos.BasketDto;
using Arowolo_Delivery_Project.Dtos.OrderDtos;
using Arowolo_Delivery_Project.Enums;
using Arowolo_Delivery_Project.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Arowolo_Delivery_Project.Services.OrderService
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


        public async Task ConfirmOrder(Guid OrderId, string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser == null)
            {
                throw new Exception("User is not active");
            }

            var order = await _context.Order.FirstOrDefaultAsync( order => order.Id == OrderId);

            if (order == null)
            {
                throw new ArgumentNullException("Order can't be found");
            }

            order.Status = Status.Delivered;

            await _context.SaveChangesAsync();
        }

        public async Task<List<GetOrderInfoDto>> GetOrder(string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser is null)
            {
                throw new Exception("User is not active");
            }

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

        public async Task<GetOrderDto> GetOrderById(Guid OrderId, string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser is null)
            {
                throw new Exception("User is not active");
            }

            var order = await _context.Order.Include(order => order.Baskets ).ThenInclude(order => order.Dish).FirstOrDefaultAsync( order => order.Id == OrderId && order.UserId == currentUser.Id );

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
            return getOrder;        }

        public async Task PostOrder(CreateOrderDto model, string UserId)
        {
            var currentUser = await _userManager.FindByIdAsync(UserId);

            if (currentUser == null)
            {
                throw new Exception("User no found");
            }

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
                OrderTime = DateTime.Now,
                Address = model.Address,
                UserId = currentUser.Id,
                Status = Status.InProcess,
                Price = totalPrice,
            };

            foreach (var item in userBasket)
            {
                item.Order = newOrder;
                item.Order.Id = newOrder.Id;
            }

            _context.Order.Add(newOrder);
            // probably add the orderid to the basket to update it 
            await _context.SaveChangesAsync();
        }
    }
}
