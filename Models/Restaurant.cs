using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace startup_trial.Models
{
    public class Restaurant : IdentityUser<Guid>
    {
        //public Guid Id { get; set; }

        public string RestaurantName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        //public string Email { get; set; } = string.Empty;

        //public string PhoneNumber { get; set; } = string.Empty;

        public DateTime startingTime { get; set; }

        public DateTime ClosingTime { get; set; }

        public ICollection<RestaurantRating> CostumerRates { get; set; } = new List<RestaurantRating>();

    }
}
