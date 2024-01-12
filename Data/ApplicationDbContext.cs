using Arowolo_Delivery_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Arowolo_Delivery_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public override DbSet<User> Users { get; set; }    
        public override DbSet<Role> Roles {  get; set; }
        public DbSet<LogoutToken> LogoutTokens { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
