using CarService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarService.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceType> ServiceType { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<ServiceShoppingCart> ServiceShoppingCart { get; set; }
        public DbSet<ServiceHeader> ServiceHeader { get; set; }
        public DbSet<ServiceDetails> ServiceDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
    
}
