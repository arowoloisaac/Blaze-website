using Arowolo_Delivery_Project.Dtos.BasketDto;
using Arowolo_Delivery_Project.Enums;

namespace Arowolo_Delivery_Project.Dtos.OrderDtos
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime OrderTime { get; set; }

        public Status Status { get; set; } = Status.Delivered;

        public ICollection<DishBasketDto> Dishes { get; set; }

        public double Price { get; set; }

        public string Address { get; set; } = string.Empty;

        //public List<DishBasketDto> Dishes { get; internal set; }
    }
}
