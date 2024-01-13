using startup_trial.Enums;

namespace startup_trial.Dtos.OrderDtos
{
    public class GetOrderInfo
    {
        public Guid Id { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime OrderTime { get; set; }

        public Status Status { get; set; } = Status.Delivered;

        public int Price { get; set; }
    }
}
