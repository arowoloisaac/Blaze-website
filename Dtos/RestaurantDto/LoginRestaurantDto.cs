namespace startup_trial.Dtos.RestaurantDto
{
    public class LoginRestaurantDto
    {
        public class LoginUserDto
        {
            public string Email { get; set; } = "user@example.com";

            public string Password { get; set; }
        }
    }
}
