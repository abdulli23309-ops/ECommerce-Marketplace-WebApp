using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Application.Services.Admin;
using ECommerce.Application.Services.Catalog;
using ECommerce.Application.Services.Orders;
using ECommerce.Application.Services.Refunds;
using ECommerce.Application.Services.Returns;
using ECommerce.Application.Services.Reviews;
using ECommerce.Application.Services.Seller;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPermissionGroupService, PermissionGroupService>();
            services.AddScoped<IRolePermissionGroupService, RolePermissionGroupService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IReturnService, ReturnService>();
            services.AddScoped<IRefundService, RefundService>();
            services.AddScoped<IAdminService, AdminService>();

            return services;
        }
    }
}