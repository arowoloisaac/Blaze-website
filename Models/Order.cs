using startup_trial.Enums;

namespace startup_trial.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime OrderTime { get; set; }

        public Status Status { get; set; } = Status.InProcess;

        public double Price { get; set; }

        //to be calculated later based on the business model
        public double DeliveryFee { get; set; }

        public ICollection<Basket> Baskets { get; set; }

        public string Address { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public Guid RestaurantId { get; set; }

        //public Driver? Driver { get; set; }

        public Guid DriverId { get; set; }
    }
}
