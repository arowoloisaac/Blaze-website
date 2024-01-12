namespace Arowolo_Delivery_Project.Models
{
    public class LogoutToken
    {
        public Guid Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Guid Identifier { get; set; }
    }
}
