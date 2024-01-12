namespace Arowolo_Delivery_Project.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public int Value { get; set; }

        public Guid DishId { get; set; }

        public Guid UserId { get; set; }
    }
}
