using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext:DbContext
    {
        public DbSet<Coupon> Coupons { get; set;}

        public DiscountContext(DbContextOptions<DiscountContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = 1, ProductName = "IPhone X",Description = "IPhone x discount" , Amount = 150},
                new Coupon { Id = 2, ProductName = "IPhone 12",Description = "IPhone 12 discount" , Amount = 100}
                );
        }
    }
}
