using Arowolo_Delivery_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using startup_trial.Models;
using System.Runtime.Intrinsics.X86;

namespace Arowolo_Delivery_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public override DbSet<User> Users { get; set; }
        //public override DbSet<Role> Roles {  get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<LogoutToken> LogoutTokens { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<RestaurantRating> restaurantRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
