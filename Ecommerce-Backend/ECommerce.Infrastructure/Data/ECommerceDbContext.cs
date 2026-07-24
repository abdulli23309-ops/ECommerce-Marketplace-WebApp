using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }

        // Authentication & Authorization
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<PermissionGroupPermission> PermissionGroupPermissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Seller
        public DbSet<SellerProfile> SellerProfiles { get; set; }
        public DbSet<Store> Stores { get; set; }

        // Catalog
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        // Cart
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Address
        public DbSet<Address> Addresses { get; set; }

        // Orders
        public DbSet<ParentOrder> ParentOrders { get; set; }
        public DbSet<SellerOrder> SellerOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentTrackingHistory> ShipmentTrackingHistories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<ReturnImage> ReturnImages { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        // Payment
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all Fluent API configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);
        }
    }
}//Entities define the structure. OnModelCreating adds rules. The tools generate migration code from the combined model. Update-Database runs that code.