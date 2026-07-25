using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceDbContext _context;
        public OrderRepository(ECommerceDbContext context) => _context = context;

        public async Task AddParentOrderAsync(ParentOrder order)
            => await _context.ParentOrders.AddAsync(order);

        public async Task AddSellerOrderAsync(SellerOrder order)
            => await _context.SellerOrders.AddAsync(order);

        public async Task AddOrderItemAsync(OrderItem item)
            => await _context.OrderItems.AddAsync(item);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<ParentOrder>> GetOrdersByUserIdAsync(Guid userId)
    => await _context.ParentOrders
        .Include(po => po.SellerOrders)
            .ThenInclude(so => so.OrderItems)
        .Include(po => po.SellerOrders)
            .ThenInclude(so => so.Shipment)
                .ThenInclude(s => s!.TrackingHistories)   // added
        .Include(po => po.Payment)
        .Where(po => po.CustomerId == userId)
        .ToListAsync();

        public async Task<IEnumerable<ParentOrder>> GetAllAsync()
            => await _context.ParentOrders
                .Include(po => po.SellerOrders)
                    .ThenInclude(so => so.OrderItems)
                .ToListAsync();
        public async Task<int> GetOrderCountAsync()
    => await _context.ParentOrders.CountAsync();

        public async Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId)
            => await _context.OrderItems
                .Include(oi => oi.SellerOrder)
                    .ThenInclude(so => so.ParentOrder)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId);

        public async Task<IEnumerable<SellerOrder>> GetSellerOrdersByStoreIdAsync(Guid storeId)
            => await _context.SellerOrders
                .Include(so => so.ParentOrder)
                .Include(so => so.OrderItems)
                .Include(so => so.Shipment)
                    .ThenInclude(s => s!.TrackingHistories)
                .Where(so => so.StoreId == storeId)
                .ToListAsync();

        public async Task<Guid?> GetStoreIdBySellerOrderIdAsync(Guid sellerOrderId)
            => await _context.SellerOrders
                .Where(so => so.Id == sellerOrderId)
                .Select(so => (Guid?)so.StoreId)
                .FirstOrDefaultAsync();
    }
}