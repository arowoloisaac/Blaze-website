using System.ComponentModel.DataAnnotations.Schema;

namespace startup_trial.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public int Count { get; set; }

        public Dish Dish { get; set; }

        public Order? Order { get; set; }

        public User User { get; set; }

        public Guid RestaurantId { get; set; }

        //public Customer Customer { get; set; }


    }
}
