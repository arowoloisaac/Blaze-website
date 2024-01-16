using startup_trial.Enums;

namespace startup_trial.Dtos.UserDtos
{
    public class CustomerProfileDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; } = "user@example.com";

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
