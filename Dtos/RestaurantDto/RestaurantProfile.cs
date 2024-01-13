using startup_trial.Enums;

namespace startup_trial.Dtos.RestaurantDto
{
    public class RestaurantProfile
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = "user@example.com";

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime OpenTime { get; set; }

        public DateTime CloseTime { get; set; }
    }
}
