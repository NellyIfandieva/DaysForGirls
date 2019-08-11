using DaysForGirls.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Data
{
    public class DaysForGirlsDbContext : IdentityDbContext<DaysForGirlsUser, IdentityRole, string>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCart> ProductsCarts { get; set; }
        public DbSet<ProductOrder> ProductsOrders { get; set; }
        //public DbSet<ProductSale> ProductsSales { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Quantity> Quantities { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public DaysForGirlsDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
