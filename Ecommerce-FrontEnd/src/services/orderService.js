import axiosInstance from "./axiosInstance";

export const placeOrder = async (addressId) => {
  const response = await axiosInstance.post("/orders/checkout", { addressId });
  return response.data;
};

export const fetchOrders = async () => {
  const response = await axiosInstance.get("/orders");
  return response.data; // array of ParentOrderDto
};