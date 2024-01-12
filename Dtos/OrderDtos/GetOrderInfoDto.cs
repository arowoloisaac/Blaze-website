using Arowolo_Delivery_Project.Enums;

namespace Arowolo_Delivery_Project.Dtos.OrderDtos
{
    public class GetOrderInfoDto
    {
        public Guid Id { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime OrderTime { get; set; }

        public Status Status { get; set; } = Status.InProcess;

        public double Price { get; set; }
    }
}
