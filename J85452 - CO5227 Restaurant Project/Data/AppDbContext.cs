using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.Data;
using J85452___CO5227_Restaurant_Project.CS;
using System.ComponentModel.DataAnnotations.Schema;

namespace J85452___CO5227_Restaurant_Project.Data
{
    public class AppDbContext : IdentityDbContext<AppUserClass>
    {
        // App Database Context for storing database data
        public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<MenuClass> Menu { get; set; }
        public DbSet<CustomerClass> Customer { get; set; }
        public DbSet<OfferClass> Offer { get; set; }
        public DbSet<OrderHistoryClass> OrderHistory { get; set; }
        public DbSet<RestaurantClass> Restaurant { get; set; }

        // Baskets
        public DbSet<CheckoutCustomerClass> CheckoutCustomer { get; set; }
        public DbSet<BasketClass> Basket { get; set; }
        public DbSet<BasketItemClass> BasketItem { get; set; }

        // Order history and checkout (additional tables)
        public DbSet<OrderItemClass> OrderItem { get; set; }
        [NotMapped]
        public DbSet<CheckoutItemClass> CheckoutItem { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BasketItemClass>().HasKey(t => new { t.ItemID, t.BasketID });
            builder.Entity<OrderItemClass>().HasKey(t => new {t.ItemID, t.OrderID});
        }
    }
}
