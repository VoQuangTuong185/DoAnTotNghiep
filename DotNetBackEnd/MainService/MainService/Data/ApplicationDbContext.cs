using Microsoft.EntityFrameworkCore;
using DoAnTotNghiep.Models.Entities;
using WebAppAPI.Models.Entities;
using WebAppAPI.Models.Entities.WebAppAPI.Models.Entities;
using doantotnghiep.Models.Entities;

namespace WebAppAPI.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAPI> UserAPIs { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<VIP> VIPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAPI>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.HasOne(ur => ur.user).WithMany(u => u.UserAPIs).HasPrincipalKey(u => u.Id).HasForeignKey(ur => ur.UserId);
                entity.HasOne(ur => ur.role).WithMany(u => u.UserRoles).HasPrincipalKey(u => u.Id).HasForeignKey(ur => ur.RoleId);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(ur => ur.Id);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(ur => ur.Id);
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(pr => pr.Id);
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(or => or.Id);
            });
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(cr => new { cr.UserId, cr.ProductId });
                entity.HasOne(ur => ur.User).WithMany(u => u.U_carts).HasPrincipalKey(u => u.Id).HasForeignKey(ur => ur.UserId);
                entity.HasOne(ur => ur.product).WithMany(u => u.P_carts).HasPrincipalKey(u => u.Id).HasForeignKey(ur => ur.ProductId);
            });
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(fb => new { fb.UserId, fb.OrderId, fb.ProductId });
                entity.HasOne(fb => fb.users).WithMany(f => f.feedbacks).HasPrincipalKey(f => f.Id).HasForeignKey(fb => fb.UserId);
                entity.HasOne(fb => fb.orders).WithMany(f => f.feedbacks).HasPrincipalKey(f => f.Id).HasForeignKey(fb => fb.OrderId);
                entity.HasOne(fb => fb.product).WithMany(f => f.feedbacks).HasPrincipalKey(f => f.Id).HasForeignKey(fb => fb.ProductId);
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(fb => new { fb.OrderId, fb.ProductId });
                entity.HasOne(fb => fb.orders).WithMany(f => f.orderDetails).HasPrincipalKey(f => f.Id).HasForeignKey(fb => fb.OrderId);
                entity.HasOne(fb => fb.product).WithMany(f => f.orderDetails).HasPrincipalKey(f => f.Id).HasForeignKey(fb => fb.ProductId);
            });
        }
    }
}
