using Arowolo_Delivery_Project.Enums;

namespace Arowolo_Delivery_Project.Dtos.UserDtos
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = string.Empty;

        public DateTime BirthdDate { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

        public string Email { get; set; } = "user@example.com";

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber {  get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
