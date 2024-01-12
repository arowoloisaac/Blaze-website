using Microsoft.AspNetCore.Identity;

namespace Arowolo_Delivery_Project.Models
{
    public class Role : IdentityRole<Guid>, IBaseEntity
    {
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
