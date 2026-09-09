import axios from "axios";

export const api = axios.create({
  baseURL: "/api",
  withCredentials: true
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("guestToken");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});