using startup_trial.Enums;

namespace startup_trial.Dtos.RestaurantDto.RestaurantDishDto
{
    public class AddRestaurantDishDto
    {
        public string Name { get; set; } = "Rice";
        public string Description { get; set; } = "a good tasting food";
        public double Price { get; set; } = 100;
        public bool IsVegetarian { get; set; } = true;
        public Category Category { get; set; } = Category.Wok;
        //public int Rating { get; set; } = 8;
        public string PhotoUrl { get; set; } = string.Empty;
        //public Guid Res
    }
}
