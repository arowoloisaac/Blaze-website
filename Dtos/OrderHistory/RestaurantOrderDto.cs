using startup_trial.Enums;

namespace startup_trial.Dtos.OrderHistory
{
    public class RestaurantOrderDto
    {
        public Guid Id { get; set; }

        public DateTime OrderTime { get; set; }

        public Status Status { get; set; } = Status.InProcess;

        public double Price { get; set; }
    }
}
