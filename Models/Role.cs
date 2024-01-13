using Microsoft.AspNetCore.Identity;

namespace startup_trial.Models
{
    public class Role : IdentityRole<Guid>, IBaseEntity
    {
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
