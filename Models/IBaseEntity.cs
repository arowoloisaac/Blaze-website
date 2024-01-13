using System.ComponentModel.DataAnnotations;

namespace startup_trial.Models
{
    public interface IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ModifyDateTime { get; set; }

        public DateTime? DeleteDate { get; set; }
    }
}
