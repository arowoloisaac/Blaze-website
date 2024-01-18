using startup_trial.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace startup_trial.Models
{
    public class Driver : User
    {
        //public Guid Id { get; set; }

        //public string FullName { get; set; } = string.Empty;

        //public string Email { get; set; } = string.Empty;

        //public DateTime BirthDate { get; set; }

        //public Gender Gender { get; set; } = Gender.Male;

        //public string PhoneNumber { get; set; } = string.Empty;

        //public Acquire Acquire { get; set; } = Acquire.Accept;

        public string CarType { get; set; } = string.Empty;

        public string CarColor { get; set; } = string.Empty;

        [Required]
        public string PlateNumber { get; set; } = string.Empty;

        //amount earned
        //public double Amount { get; set; }

        //number of accepted orders
        //public int Accepted {  get; set; }

        //public ICollection<RestaurantRating> Restaurants { get; set;}
    }
}
