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
export const uploadProductImage = async (productId, file) => {
  const formData = new FormData();
  formData.append('file', file);
  const response = await axiosInstance.post(`/products/${productId}/images`, formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
  return response.data; // ProductImageDto { id, imageUrl, sortOrder }
};

export const deleteProductImage = async (productId, imageId) => {
  await axiosInstance.delete(`/products/${productId}/images/${imageId}`);
};