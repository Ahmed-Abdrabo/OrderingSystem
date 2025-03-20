using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Core.Entities.Identity;
using OrderingSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Repository.Data
{
    public class AppDbContext : IdentityDbContext<Customer>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Customer - Address (One-to-One)
            builder.Entity<Customer>()
                .HasOne(c => c.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);           
        
            builder.Entity<Address>().ToTable("Addresses");
        }
    }
}
