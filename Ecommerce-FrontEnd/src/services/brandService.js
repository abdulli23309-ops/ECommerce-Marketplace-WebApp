import axiosInstance from "./axiosInstance";

export const fetchBrands = async () => {
  const response = await axiosInstance.get("/brands");
  return response.data;
};