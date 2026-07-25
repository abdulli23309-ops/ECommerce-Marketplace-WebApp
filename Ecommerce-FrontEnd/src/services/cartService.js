import axiosInstance from "./axiosInstance";

export const fetchCart = async () => {
  const response = await axiosInstance.get("/cart");
  return response.data; // { cartId, items: [...] }
};

export const addToCart = async (productId, quantity) => {
  const response = await axiosInstance.post("/cart/add", { productId, quantity });
  return response.data;
};

export const updateCartItemQuantity = async (cartItemId, quantity) => {
  const response = await axiosInstance.put(`/cart/items/${cartItemId}`, quantity, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data;
};

export const removeCartItem = async (cartItemId) => {
  await axiosInstance.delete(`/cart/remove/${cartItemId}`);
};

export const clearCart = async () => {
  await axiosInstance.post("/cart/clear");
};