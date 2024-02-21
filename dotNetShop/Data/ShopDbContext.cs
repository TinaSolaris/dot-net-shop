using Microsoft.EntityFrameworkCore;
using dotNetShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace dotNetShop.Data
{
    public class ShopDbContext : IdentityDbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderUser> OrderUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentMethod)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);         

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Article)
                .WithMany()
                .HasForeignKey(oi => oi.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderUser>()
                .HasOne(ou => ou.User)
                .WithMany()
                .HasForeignKey(ou => ou.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}