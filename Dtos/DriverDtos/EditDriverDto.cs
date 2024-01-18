using startup_trial.Enums;
using System.ComponentModel.DataAnnotations;

namespace startup_trial.Dtos.DriverDtos
{
    public class EditDriverDto
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string CarType { get; set; } = string.Empty;

        public string CarColor { get; set; } = string.Empty;

        public string PlateNumber { get; set; } = string.Empty;
    }
}
