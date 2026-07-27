import axiosInstance from "./axiosInstance";

export const placeOrder = async (addressId) => {
  const response = await axiosInstance.post("/orders/checkout", { addressId });
  return response.data;
};

export const fetchOrders = async () => {
  const response = await axiosInstance.get("/orders");
  return response.data; // array of ParentOrderDto
};
export const fetchOrderById = async (orderId) => {
  const response = await axiosInstance.get(`/orders/${orderId}`);
  return response.data;
};