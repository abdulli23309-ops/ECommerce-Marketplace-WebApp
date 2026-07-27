import axiosInstance from "./axiosInstance";

export const fetchApprovedProducts = async (params = {}) => {
  const response = await axiosInstance.get("/products/all", {
    params: {
      page: params.page || 1,
      pageSize: params.pageSize || 12,
      categoryId: params.categoryId || undefined,
      subCategoryId: params.subCategoryId || undefined,
      brandId: params.brandId || undefined,
      minPrice: params.minPrice || undefined,
      maxPrice: params.maxPrice || undefined,
      search: params.search || undefined,
      sortBy: params.sortBy || undefined,
    },
  });
  return response.data;
};
export const fetchProductById = async (productId) => {
  const response = await axiosInstance.get(`/products/${productId}`);
  return response.data;
};
export const fetchProductsByStore = async (storeId) => {
  const response = await axiosInstance.get(`/products/store/${storeId}`); // we'll create this backend endpoint
  return response.data;
};