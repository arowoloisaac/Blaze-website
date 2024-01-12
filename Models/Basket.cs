using System.ComponentModel.DataAnnotations.Schema;

namespace Arowolo_Delivery_Project.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public int Count { get; set; }

        public Dish Dish { get; set; }

        public Order? Order { get; set; }

        public User User { get; set; }
    }
}
