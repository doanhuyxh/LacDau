using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LacDau.Models;

namespace LacDau.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Trademark> Trademark { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImg> ProductImg { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasMany(p=>p.ProductImg).WithOne(pi => pi.Product).HasForeignKey(p=>p.ProductId);
        }
    }
}
