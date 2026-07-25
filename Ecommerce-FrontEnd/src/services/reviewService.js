import axiosInstance from "./axiosInstance";

export const fetchProductReviews = async (productId) => {
  const response = await axiosInstance.get(`/reviews/product/${productId}`);
  return response.data;
};