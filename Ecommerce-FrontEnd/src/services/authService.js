import axiosInstance from "./axiosInstance";

export const loginUser = async (email, password) => {
  const response = await axiosInstance.post("/Account/login", { email, password });
  return response.data; // { succeeded, accessToken, refreshToken, message, errors }
};

export const registerUser = async (fullName, email, password, confirmPassword) => {
  const response = await axiosInstance.post("/Account/register", {
    fullName,
    email,
    password,
    confirmPassword,
  });
  return response.data;
};

export const refreshAccessToken = async (refreshToken) => {
  const response = await axiosInstance.post("/Account/refresh-token", refreshToken, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data; // { succeeded, accessToken, refreshToken, message }
};

export const logoutUser = async (refreshToken) => {
  await axiosInstance.post("/Account/logout", refreshToken, {
    headers: { "Content-Type": "application/json" },
  });
};
export const updateProfile = async (fullName, email) => {
  const response = await axiosInstance.put("/Account/profile", { fullName, email });
  return response.data;
};