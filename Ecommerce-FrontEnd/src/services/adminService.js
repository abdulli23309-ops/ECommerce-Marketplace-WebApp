import axiosInstance from "./axiosInstance";

export const getSellers = async () => {
  const response = await axiosInstance.get("/admin/sellers");
  return response.data;
};

export const approveSeller = async (sellerId) => {
  await axiosInstance.put(`/admin/sellers/${sellerId}/approve`);
};

export const rejectSeller = async (sellerId, reason) => {
  await axiosInstance.put(`/admin/sellers/${sellerId}/reject`, reason, {
    headers: { "Content-Type": "application/json" },
  });
};

export const getProducts = async () => {
  const response = await axiosInstance.get("/admin/products");
  return response.data;
};

export const updateProductStatus = async (productId, status) => {
  await axiosInstance.put(`/admin/products/${productId}/status`, `"${status}"`, {
    headers: { "Content-Type": "application/json" },
  });
};

export const getReturns = async () => {
  const response = await axiosInstance.get("/admin/returns");
  return response.data;
};

export const approveReturn = async (returnId) => {
  await axiosInstance.put(`/admin/returns/${returnId}/approve`);
};

export const rejectReturn = async (returnId) => {
  await axiosInstance.put(`/admin/returns/${returnId}/reject`);
};

export const createRefund = async (refundData) => {
  const response = await axiosInstance.post("/admin/refunds", refundData);
  return response.data;
};

export const getPayments = async () => {
  const response = await axiosInstance.get("/admin/payments");
  return response.data;
};

// NEW — Admin dashboard stats
export const getAdminStats = async () => {
  const response = await axiosInstance.get("/admin/stats");
  return response.data;
};