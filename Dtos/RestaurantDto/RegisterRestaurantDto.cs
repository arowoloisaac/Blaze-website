using startup_trial.Enums;

namespace startup_trial.Dtos.RestaurantDto
{
    public class RegisterRestaurantDto
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = "user@example.com";

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime OpeningTime { get; set; }

        public DateTime ClosingTime { get; set; }
    }
}
