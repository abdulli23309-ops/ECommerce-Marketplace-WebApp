import axiosInstance from "./axiosInstance";

export const fetchSellerProducts = async () => {
  const response = await axiosInstance.get("/products");
  return response.data; // array of ProductDto (seller's own products)
};

export const createProduct = async (productData) => {
  const response = await axiosInstance.post("/products", productData);
  return response.data;
};

export const updateProduct = async (productId, productData) => {
  const response = await axiosInstance.put(`/products/${productId}`, productData);
  return response.data;
};

// Optional: soft delete – will just call a delete endpoint when available
export const deleteProduct = async (productId) => {
  // backend may not support DELETE yet; we can use PUT to archive or just skip
  // we'll implement later if needed
};