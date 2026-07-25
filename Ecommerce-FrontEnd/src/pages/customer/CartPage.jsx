import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import {
  fetchCart,
  updateCartItemQuantity,
  removeCartItem,
  clearCart,
} from "../../services/cartService";

const CartPage = () => {
  const navigate = useNavigate();
  const [cart, setCart] = useState(null);
  const [loading, setLoading] = useState(true);

  const loadCart = async () => {
    try {
      const data = await fetchCart();
      setCart(data);
    } catch (err) {
      console.error("Failed to load cart", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCart();
  }, []);

  const handleQuantityChange = async (cartItemId, newQuantity) => {
    if (newQuantity < 1) return;
    try {
      await updateCartItemQuantity(cartItemId, newQuantity);
      loadCart();
    } catch (err) {
      console.error("Failed to update quantity", err);
    }
  };

  const handleRemove = async (cartItemId) => {
    try {
      await removeCartItem(cartItemId);
      loadCart();
    } catch (err) {
      console.error("Failed to remove item", err);
    }
  };

  const handleClearCart = async () => {
    try {
      await clearCart();
      setCart({ ...cart, items: [] });
    } catch (err) {
      console.error("Failed to clear cart", err);
    }
  };

  const calculateTotal = () => {
    if (!cart?.items) return 0;
    return cart.items.reduce(
      (sum, item) => sum + item.unitPrice * item.quantity,
      0
    );
  };

  if (loading) {
    return <div style={{ padding: "3rem", color: "#666" }}>Loading cart...</div>;
  }

  if (!cart || cart.items?.length === 0) {
    return (
      <div className="cart-page">
        <div className="cart-empty">
          <h2>Your cart is empty</h2>
          <p>Add some products to get started.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="cart-page">
      <h1 className="section-title">Shopping Cart</h1>

      <div className="cart-items">
        {cart.items.map((item) => (
          <div className="cart-item" key={item.cartItemId}>
            <div className="cart-item-details">
              <p className="cart-item-name">{item.productName}</p>
              <p className="cart-item-price">PKR {item.unitPrice.toLocaleString()}</p>
            </div>

            <div className="cart-item-actions">
              <div className="quantity-control">
                <button
                  className="quantity-btn"
                  onClick={() => handleQuantityChange(item.cartItemId, item.quantity - 1)}
                  disabled={item.quantity <= 1}
                >
                  −
                </button>
                <span className="quantity-value">{item.quantity}</span>
                <button
                  className="quantity-btn"
                  onClick={() => handleQuantityChange(item.cartItemId, item.quantity + 1)}
                >
                  +
                </button>
              </div>
              <button className="btn-remove" onClick={() => handleRemove(item.cartItemId)}>
                Remove
              </button>
            </div>
          </div>
        ))}
      </div>

      <div className="cart-summary">
        <div>
          <button className="btn-remove" onClick={handleClearCart}>
            Clear Cart
          </button>
        </div>
        <div className="cart-total">
          Total: PKR {calculateTotal().toLocaleString()}
        </div>
        <button
          className="btn-checkout"
          onClick={() => navigate("/checkout")}
        >
          Proceed to Checkout
        </button>
      </div>
    </div>
  );
};

export default CartPage;