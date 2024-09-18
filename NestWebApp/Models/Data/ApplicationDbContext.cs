using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NestWebApp.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<MailSettings> MailSettings { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<SocialMedia> SocialMedia { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}
