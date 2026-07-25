import axiosInstance from "./axiosInstance";

export const fetchApprovedProducts = async (page = 1, pageSize = 8) => {
  const response = await axiosInstance.get("/products/all", {
    params: { page, pageSize },
  });
  return response.data; // { items, totalCount, page, pageSize, totalPages }
};

export const fetchProductById = async (productId) => {
  const response = await axiosInstance.get(`/products/${productId}`);
  return response.data;
};
export const fetchProductsByStore = async (storeId) => {
  const response = await axiosInstance.get(`/products/store/${storeId}`); // we'll create this backend endpoint
  return response.data;
};