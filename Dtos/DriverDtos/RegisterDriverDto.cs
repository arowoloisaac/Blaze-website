using startup_trial.Dtos.UserDtos;

namespace startup_trial.Dtos.DriverDtos
{
    public class RegisterDriverDto : RegisterCustomerDto
    {
        public string CarType { get; set; } = string.Empty;

        public string CarColor { get; set; } = string.Empty;

        public string PlateNumber { get; set; } = string.Empty;
    }
}
