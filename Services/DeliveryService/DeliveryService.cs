using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using startup_trial.Data;
using startup_trial.Dtos.DeliveryDto;
using startup_trial.Enums;
using startup_trial.Models;

namespace startup_trial.Services.DeliveryService
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DeliveryService(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }


        public async Task<string> AcquireOrder(Acquire arquire, string driverId)
        {
            var deliverer = await _userManager.FindByIdAsync(driverId);
            var order = await _context.Order.Where(order => order.Status == Status.InProcess && order.RestaurantId != null).FirstOrDefaultAsync(); //order.RestaurantId == restaurantId && order.Status == Status.InProcess &&  && order.RestaurantId != null
            var inTransitOrdersCount = await _context.Order.Where(order => order.Status == Status.InTransit && order.RestaurantId != null && order.DriverId == deliverer.Id).CountAsync();

            if (deliverer == null)
            {
                throw new Exception("User not active");
            }

            if (deliverer is Driver driver)
            {
                //var checkStatus = Status.InTransit;
                if (inTransitOrdersCount == 0)
                {
                    if (arquire == Acquire.Accept)
                    {
                        order.Status = Status.InTransit;
                        //order.Driver.Id = deliverer.Id;
                        //order.Driver.Acquire = arquire;
                        order.DriverId = deliverer.Id;
                        //var Accepted = arquire;
                        await _context.SaveChangesAsync();

                        return ("Accepted");
                    }

                    else
                    {
                        return ("Rejected");
                    }
                }

                else
                {
                    return ("Already with an order intransit, deliver that first!");
                }
            }

            else
            {
                return ("Unauthorized to perform such action!");
            }
            
        }

        public async Task<List<DeliveryProfileDto>> DeliveryProfile(string UserId)
        {
            var deliverer = await _userManager.FindByIdAsync(UserId);

            //var portFolio = await _context.Order.Where(order => order.Status == Status.Delivered && order.DriverId == deliverer.Id).ToListAsync();
            if (deliverer is Driver driver)
            {
                var portfolio = await _context.Order.Where(order => order.DriverId == deliverer.Id && order.Status == Status.Delivered).ToListAsync();



                var lst = portfolio.Select(folio => new DeliveryProfileDto
                {
                    //name of deliverer
                    Name = deliverer.FullName,
                    Earned = portfolio.Sum(order => order.DeliveryFee),
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


/**/