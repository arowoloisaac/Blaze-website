using Arowolo_Delivery_Project.Enums;

namespace Arowolo_Delivery_Project.Dtos.UserDtos
{
    public class EditUserDto
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
