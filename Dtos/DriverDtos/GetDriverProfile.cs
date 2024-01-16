using startup_trial.Enums;
using System.ComponentModel.DataAnnotations;

namespace startup_trial.Dtos.DriverDtos
{
    public class GetDriverProfile
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

        public string PhoneNumber { get; set; } = string.Empty;

        public string CarType { get; set; } = string.Empty;

        public string CarColor { get; set; } = string.Empty;

        public string PlateNumber { get; set; } = string.Empty;
    }
}
