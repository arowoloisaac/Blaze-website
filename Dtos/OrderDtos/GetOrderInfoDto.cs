using startup_trial.Enums;

namespace startup_trial.Dtos.OrderDtos
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
