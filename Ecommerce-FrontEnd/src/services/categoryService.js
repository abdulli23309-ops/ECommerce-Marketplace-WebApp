import axiosInstance from "./axiosInstance";

export const fetchCategories = async () => {
  const response = await axiosInstance.get("/categories");
  return response.data;
};

export const fetchSubCategories = async (categoryId) => {
  const response = await axiosInstance.get(`/categories/${categoryId}/subcategories`);
  return response.data;
};