import axiosInstance from "./axiosInstance";

export const createShipment = async (sellerOrderId, trackingNumber, carrier) => {
  const response = await axiosInstance.post("/shipments", {
    sellerOrderId,
    trackingNumber,
    carrier,
  });
  return response.data;
};

export const updateShipmentStatus = async (shipmentId, status, location) => {
  const response = await axiosInstance.put(`/shipments/${shipmentId}/status`, {
    status,
    location: location || "",
  });
  return response.data;
};

export const getShipmentByOrder = async (sellerOrderId) => {
  const response = await axiosInstance.get(`/shipments/order/${sellerOrderId}`);
  return response.data;
};