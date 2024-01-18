using startup_trial.Enums;

namespace startup_trial.Dtos.DishDto
{
    public class GetDishDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool IsVegetarian { get; set; }
        public Category Category { get; set; }
        public double Rating { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
