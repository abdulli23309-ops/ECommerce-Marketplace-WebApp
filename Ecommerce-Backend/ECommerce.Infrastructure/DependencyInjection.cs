using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Admin;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Repositories.Catalog;
using ECommerce.Infrastructure.Repositories.Orders;
using ECommerce.Infrastructure.Repositories.Refunds;
using ECommerce.Infrastructure.Repositories.Returns;
using ECommerce.Infrastructure.Repositories.Reviews;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IShipmentRepository, ShipmentRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReturnRepository, ReturnRepository>();
            services.AddScoped<IRefundRepository, RefundRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionGroupRepository, RolePermissionGroupRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionGroupRepository, PermissionGroupRepository>();

            // Infrastructure-only services
            services.AddScoped<IPasswordHasherService, BCryptPasswordHasherService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}