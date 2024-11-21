using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WEBSAIGONGLISTEN.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<AdditionalUserInfo> AdditionalUserInfos { get; set; }

        public DbSet<OrderStatistics> OrderStatistics { get; set; }

        public DbSet<TourDay> TourDays { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<ThongBao> ThongBaos { get; set; }

        public DbSet<BookingHistory> BookingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Gọi phương thức cơ bản để thiết lập các thực thể Identity
            base.OnModelCreating(modelBuilder);

            // Cấu hình khóa ngoại với DeleteBehavior.NoAction cho Product
            modelBuilder.Entity<TourDay>()
                .HasOne(t => t.Product)
                .WithMany(p => p.TourDays)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // Category vẫn giữ DeleteBehavior.Cascade (nếu cần)
            modelBuilder.Entity<TourDay>()
                .HasOne(t => t.Category)
                .WithMany(c => c.TourDays)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }   
    }
}
