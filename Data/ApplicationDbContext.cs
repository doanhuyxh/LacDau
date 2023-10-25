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
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Merchant> Merchant { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentDestination> PaymentDestination { get; set; }
        public DbSet<PaymentNotification> PaymentNotification { get; set; }
        public DbSet<PaymentSignature> PaymentSignature { get; set; }
        public DbSet<PaymentTransaction> PaymentTransaction { get; set; }
        
    }
}
