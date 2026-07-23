using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IAddressRepository _addressRepo;

        public OrderService(IOrderRepository orderRepo, ICartRepository cartRepo, IAddressRepository addressRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _addressRepo = addressRepo;
        }

        public async Task<ParentOrderDto> CheckoutAsync(Guid userId, CheckoutDto dto)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId)
                        ?? throw new InvalidOperationException("Cart is empty.");

            var address = await _addressRepo.GetByIdAsync(dto.AddressId)
                          ?? throw new InvalidOperationException("Address not found.");

            // Group cart items by StoreId
            var itemsByStore = cart.CartItems
                .GroupBy(ci => ci.Product.StoreId)
                .ToList();

            // Create ParentOrder (address snapshot)
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

                    // Remove cart item
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
            return orders.Select(po => new ParentOrderDto
            {
                ParentOrderId = po.Id,
                OrderDate = po.OrderDate,
                OrderStatus = po.OrderStatus,
                TotalAmount = po.TotalAmount,
                SellerOrders = po.SellerOrders.Select(so => new SellerOrderDto
                {
                    SellerOrderId = so.Id,
                    StoreName = so.Store?.Name ?? "Deleted Store",
                    SubTotal = so.SubTotal,
                    Status = so.Status,
                    Items = so.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.ProductNameSnapshot,
                        UnitPrice = oi.UnitPriceSnapshot,
                        Quantity = oi.Quantity
                    }).ToList()
                }).ToList()
            });
        }
    }
}