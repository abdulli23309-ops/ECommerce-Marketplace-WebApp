using ECommerce.Application.DTOs.Order;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IAddressRepository _addressRepo;
        private readonly ISellerRepository _sellerRepo;

        public OrderService(IOrderRepository orderRepo, ICartRepository cartRepo,
            IAddressRepository addressRepo, ISellerRepository sellerRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _addressRepo = addressRepo;
            _sellerRepo = sellerRepo;
        }

        public async Task<ParentOrderDto> CheckoutAsync(Guid userId, CheckoutDto dto)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId)
                        ?? throw new InvalidOperationException("Cart is empty.");

            var address = await _addressRepo.GetByIdAsync(dto.AddressId)
                          ?? throw new InvalidOperationException("Address not found.");

            var itemsByStore = cart.CartItems
                .GroupBy(ci => ci.Product.StoreId)
                .ToList();

            var parentOrder = new ParentOrder
            {
                CustomerId = userId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending",
                ShippingFullName = address.FullName,
                ShippingPhone = address.PhoneNumber,
                ShippingAddressLine1 = address.AddressLine1,
                ShippingAddressLine2 = address.AddressLine2,
                ShippingCity = address.City,
                ShippingState = address.State,
                ShippingPostalCode = address.PostalCode,
                TotalAmount = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _orderRepo.AddParentOrderAsync(parentOrder);
            decimal total = 0;

            foreach (var group in itemsByStore)
            {
                var storeId = group.Key;
                decimal subTotal = 0;
                var sellerOrder = new SellerOrder
                {
                    ParentOrderId = parentOrder.Id,
                    StoreId = storeId,
                    SubTotal = 0,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };
                await _orderRepo.AddSellerOrderAsync(sellerOrder);

                foreach (var cartItem in group)
                {
                    var lineTotal = cartItem.Product.BasePrice * cartItem.Quantity;
                    subTotal += lineTotal;

                    var orderItem = new OrderItem
                    {
                        SellerOrderId = sellerOrder.Id,
                        ProductId = cartItem.ProductId,
                        ProductNameSnapshot = cartItem.Product.Name,
                        UnitPriceSnapshot = cartItem.Product.BasePrice,
                        Quantity = cartItem.Quantity,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _orderRepo.AddOrderItemAsync(orderItem);

                    _cartRepo.RemoveCartItem(cartItem);
                }

                sellerOrder.SubTotal = subTotal;
                total += subTotal;
            }

            parentOrder.TotalAmount = total;
            await _orderRepo.SaveChangesAsync();

            return new ParentOrderDto
            {
                ParentOrderId = parentOrder.Id,
                OrderDate = parentOrder.OrderDate,
                OrderStatus = parentOrder.OrderStatus,
                TotalAmount = parentOrder.TotalAmount
            };
        }

        public async Task<IEnumerable<ParentOrderDto>> GetMyOrdersAsync(Guid userId)
        {
            var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToDto);
        }

        public async Task<IEnumerable<ParentOrderDto>> GetSellerOrdersAsync(Guid userId)
        {
            var profile = await _sellerRepo.GetByUserIdAsync(userId);
            if (profile == null) return new List<ParentOrderDto>();

            var store = await _sellerRepo.GetStoreBySellerIdAsync(profile.Id);
            if (store == null) return new List<ParentOrderDto>();

            var sellerOrders = await _orderRepo.GetSellerOrdersByStoreIdAsync(store.Id);

            var parentOrders = sellerOrders
                .GroupBy(so => so.ParentOrderId)
                .Select(g => new ParentOrderDto
                {
                    ParentOrderId = g.Key,
                    OrderDate = g.First().ParentOrder.OrderDate,
                    OrderStatus = g.First().ParentOrder.OrderStatus,
                    TotalAmount = g.Sum(so => so.SubTotal),
                    SellerOrders = g.Select(so => new SellerOrderDto
                    {
                        SellerOrderId = so.Id,
                        StoreName = store.Name,
                        SubTotal = so.SubTotal,
                        Status = so.Status,
                        Items = so.OrderItems.Select(oi => new OrderItemDto
                        {
                            ProductName = oi.ProductNameSnapshot,
                            UnitPrice = oi.UnitPriceSnapshot,
                            Quantity = oi.Quantity
                        }).ToList(),
                        Shipment = so.Shipment == null ? null : new ShipmentDto
                        {
                            ShipmentId = so.Shipment.Id,
                            SellerOrderId = so.Shipment.SellerOrderId,
                            TrackingNumber = so.Shipment.TrackingNumber,
                            Carrier = so.Shipment.Carrier,
                            Status = so.Shipment.Status,
                            TrackingHistory = so.Shipment.TrackingHistories?.Select(th =>
                                new ShipmentTrackingHistoryDto
                                {
                                    Status = th.Status,
                                    Location = th.Location,
                                    Timestamp = th.Timestamp
                                }).ToList() ?? new List<ShipmentTrackingHistoryDto>()
                        }
                    }).ToList()
                })
                .ToList();

            return parentOrders;
        }

        public async Task<ParentOrderDto?> GetOrderByIdAsync(Guid userId, Guid parentOrderId)
        {
            var order = await _orderRepo.GetOrderByIdForUserAsync(parentOrderId, userId);
            return order == null ? null : MapToDto(order);
        }

        private ParentOrderDto MapToDto(ParentOrder order)
        {
            return new ParentOrderDto
            {
                ParentOrderId = order.Id,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                TotalAmount = order.TotalAmount,
                SellerOrders = order.SellerOrders.Select(so => new SellerOrderDto
                {
                    SellerOrderId = so.Id,
                    StoreName = so.Store?.Name ?? "Deleted Store",
                    SubTotal = so.SubTotal,
                    Status = so.Status,
                    Items = so.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId = oi.Id,
                        ProductName = oi.ProductNameSnapshot,
                        UnitPrice = oi.UnitPriceSnapshot,
                        Quantity = oi.Quantity
                    }).ToList(),
                    Shipment = so.Shipment == null ? null : new ShipmentDto
                    {
                        ShipmentId = so.Shipment.Id,
                        SellerOrderId = so.Shipment.SellerOrderId,
                        TrackingNumber = so.Shipment.TrackingNumber,
                        Carrier = so.Shipment.Carrier,
                        Status = so.Shipment.Status,
                        TrackingHistory = so.Shipment.TrackingHistories?.Select(th =>
                            new ShipmentTrackingHistoryDto
                            {
                                Status = th.Status,
                                Location = th.Location,
                                Timestamp = th.Timestamp
                            }).ToList() ?? new List<ShipmentTrackingHistoryDto>()
                    }
                }).ToList()
            };
        }
    }
}