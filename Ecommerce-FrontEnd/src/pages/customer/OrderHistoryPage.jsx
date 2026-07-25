import { useState, useEffect } from "react";
import { fetchOrders } from "../../services/orderService";

const OrderHistoryPage = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [expandedOrderId, setExpandedOrderId] = useState(null);

  useEffect(() => {
    const load = async () => {
      try {
        const data = await fetchOrders();
        setOrders(data);
      } catch (err) {
        console.error("Failed to load orders", err);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  const toggleOrder = (orderId) => {
    setExpandedOrderId(expandedOrderId === orderId ? null : orderId);
  };

  if (loading) {
    return <div style={{ padding: "3rem", color: "#666" }}>Loading orders...</div>;
  }

  if (orders.length === 0) {
    return (
      <div className="order-history-page">
        <div className="cart-empty">
          <h2>No orders yet</h2>
          <p>When you place an order, it will appear here.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="order-history-page">
      <h2 className="section-title">Your Orders</h2>

      {orders.map((order) => (
        <div className="order-card" key={order.parentOrderId}>
          <div
            className="order-card-header"
            onClick={() => toggleOrder(order.parentOrderId)}
          >
            <div>
              <span className="order-id">Order #{order.parentOrderId.slice(0, 8).toUpperCase()}</span>
              <span className="order-date">
                {" "}· {new Date(order.orderDate).toLocaleDateString()}
              </span>
            </div>
            <span className="order-status">{order.orderStatus}</span>
            <span className="order-total">PKR {order.totalAmount.toLocaleString()}</span>
          </div>

          {expandedOrderId === order.parentOrderId && (
            <div className="order-card-body">
              {order.sellerOrders.map((so) => (
                <div className="seller-order" key={so.sellerOrderId}>
                  <div className="seller-order-header">
                    <span className="seller-store-name">{so.storeName}</span>
                    <span className="seller-order-status">{so.status}</span>
                  </div>
                  {so.items.map((item, idx) => (
                    <div className="order-item" key={idx}>
                      <span className="order-item-name">
                        {item.productName} × {item.quantity}
                      </span>
                      <span className="order-item-price">
                        PKR {(item.unitPrice * item.quantity).toLocaleString()}
                      </span>
                    </div>
                  ))}
                  <div style={{ textAlign: "right", fontWeight: 600, marginTop: "0.5rem", color: "#000" }}>
                    Subtotal: PKR {so.subTotal.toLocaleString()}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      ))}
    </div>
  );
};

export default OrderHistoryPage;