using startup_trial.Enums;

namespace startup_trial.Dtos.UserDtos
{
    public class EditCustomerDto
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
