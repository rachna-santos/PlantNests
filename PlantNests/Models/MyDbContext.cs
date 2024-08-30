using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PlantNests.Controllers;

namespace PlantNests.Models
{
    public class MyDbContext:IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        public DbSet<Registration> registers { get; set;}
        public DbSet<Login> logins { get; set;}
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<Contact> contacts { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<SoldProduct> soldProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
