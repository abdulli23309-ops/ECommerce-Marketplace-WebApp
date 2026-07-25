import axiosInstance from "./axiosInstance";

export const fetchAddresses = async () => {
  const response = await axiosInstance.get("/address");
  return response.data;
};

export const addAddress = async (addressData) => {
  const response = await axiosInstance.post("/address", addressData);
  return response.data;
};

export const updateAddress = async (id, addressData) => {
  await axiosInstance.put(`/address/${id}`, addressData);
};

export const deleteAddress = async (id) => {
  await axiosInstance.delete(`/address/${id}`);
};

export const setDefaultAddress = async (id) => {
  await axiosInstance.put(`/address/${id}/default`);
};