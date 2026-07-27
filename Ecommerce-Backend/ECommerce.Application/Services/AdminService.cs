using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly ISellerRepository _sellerRepo;
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IShipmentRepository _shipmentRepo;
        private readonly IReturnRepository _returnRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPaymentRepository _paymentRepo;
        

        public AdminService(
            ISellerRepository sellerRepo,
            IProductRepository productRepo,
            IOrderRepository orderRepo,
            IShipmentRepository shipmentRepo,
            IReturnRepository returnRepo,
            IUserRepository userRepo,
    IPaymentRepository paymentRepo)
        {
            _sellerRepo = sellerRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _shipmentRepo = shipmentRepo;
            _returnRepo = returnRepo;
            _userRepo = userRepo;
            _paymentRepo = paymentRepo;
        }

        public async Task<AdminStatsDto> GetStatsAsync()
        {
            return new AdminStatsDto
            {
                TotalUsers = await _userRepo.GetUserCountAsync(),
                TotalSellers = await _sellerRepo.GetSellerCountAsync(),
                TotalProducts = await _productRepo.GetProductCountAsync(),
                TotalOrders = await _orderRepo.GetOrderCountAsync(),
                TotalRevenue = await _paymentRepo.GetTotalRevenueAsync(),
                PendingSellerApprovals = await _sellerRepo.GetPendingSellerCountAsync(),
                PendingProductApprovals = await _productRepo.GetPendingProductCountAsync(),
                PendingReturns = await _returnRepo.GetPendingReturnsCountAsync()
            };
        }

        public async Task<IEnumerable<SellerAdminDto>> GetSellersAsync()
        {
            var sellers = await _sellerRepo.GetAllAsync();
            return sellers.Select(sp => new SellerAdminDto
            {
                Id = sp.Id,
                UserId = sp.UserId,
                Email = sp.User.Email,
                FullName = sp.User.FullName,
                BusinessName = sp.BusinessName,
                Status = sp.Status
            });
        }

       

       

        public async Task<IEnumerable<ProductAdminDto>> GetProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            return products.Select(p => new ProductAdminDto
            {
                Id = p.Id,
                Name = p.Name,
                StoreId = p.StoreId,
                StoreName = p.Store?.Name ?? "",
                Status = p.Status,
                BasePrice = p.BasePrice,
                StockQuantity = p.StockQuantity,
                IsDeleted = p.IsDeleted
            });
        }

        public async Task UpdateProductStatusAsync(Guid productId, string status)
        {
            var product = await _productRepo.GetByIdAsync(productId)
                          ?? throw new InvalidOperationException("Product not found.");
            product.Status = status;
            product.UpdatedAt = DateTime.UtcNow;
            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ParentOrderAdminDto>> GetOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return orders.Select(po => new ParentOrderAdminDto
            {
                Id = po.Id,
                CustomerId = po.CustomerId,
                OrderDate = po.OrderDate,
                OrderStatus = po.OrderStatus,
                TotalAmount = po.TotalAmount,
                CustomerEmail = po.Customer?.Email ?? "",
                SellerOrders = po.SellerOrders.Select(so => new SellerOrderAdminDto
                {
                    Id = so.Id,
                    StoreId = so.StoreId,
                    StoreName = so.Store?.Name ?? "",
                    SubTotal = so.SubTotal,
                    Status = so.Status
                }).ToList()
            });
        }

        public async Task<IEnumerable<ShipmentAdminDto>> GetShipmentsAsync()
        {
            var shipments = await _shipmentRepo.GetAllAsync();
            return shipments.Select(s => new ShipmentAdminDto
            {
                Id = s.Id,
                SellerOrderId = s.SellerOrderId,
                TrackingNumber = s.TrackingNumber,
                Carrier = s.Carrier,
                Status = s.Status,
                CreatedAt = s.CreatedAt
            });
        }

        public async Task<IEnumerable<ReturnRequestAdminDto>> GetReturnsAsync()
        {
            var returns = await _returnRepo.GetAllAsync();
            return returns.Select(r => new ReturnRequestAdminDto
            {
                Id = r.Id,
                OrderItemId = r.OrderItemId,
                ProductName = r.OrderItem?.ProductNameSnapshot ?? "",
                Reason = r.Reason,
                Description = r.Description,
                Status = r.Status.ToString(),
                CustomerId = r.OrderItem?.SellerOrder?.ParentOrder?.CustomerId ?? Guid.Empty,
                CustomerEmail = r.OrderItem?.SellerOrder?.ParentOrder?.Customer?.Email ?? ""
            });
        }

        public async Task ApproveReturnAsync(Guid returnId)
        {
            var request = await _returnRepo.GetByIdAsync(returnId);
            if (request == null) throw new InvalidOperationException("Return not found.");
            request.Status = ReturnStatus.Approved;
            request.UpdatedAt = DateTime.UtcNow;
            _returnRepo.Update(request);
            await _returnRepo.SaveChangesAsync();
        }
        public async Task RejectSellerAsync(Guid sellerId, string? reason)
        {
            var seller = await _sellerRepo.GetByIdAsync(sellerId)
                         ?? throw new InvalidOperationException("Seller not found.");
            seller.Status = "Rejected";
            seller.RejectionReason = reason;
            seller.UpdatedAt = DateTime.UtcNow;
            _sellerRepo.UpdateProfile(seller);
            await _sellerRepo.SaveChangesAsync();
        }
        public async Task ApproveSellerAsync(Guid sellerId)
        {
            var seller = await _sellerRepo.GetByIdAsync(sellerId)
                         ?? throw new InvalidOperationException("Seller not found.");
            seller.Status = "Approved";
            seller.RejectionReason = null;
            seller.UpdatedAt = DateTime.UtcNow;
            _sellerRepo.UpdateProfile(seller);
            await _userRepo.AddUserRoleAsync(seller.UserId, "Seller");
            await _sellerRepo.SaveChangesAsync();
        }

        public async Task RejectReturnAsync(Guid returnId)
        {
            var request = await _returnRepo.GetByIdAsync(returnId);
            if (request == null) throw new InvalidOperationException("Return not found.");
            request.Status = ReturnStatus.Rejected;
            request.UpdatedAt = DateTime.UtcNow;
            await _returnRepo.SaveChangesAsync();
        }
    }
}