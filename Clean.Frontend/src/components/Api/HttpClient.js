import axios from "axios";

// Create an axios instance with base URL and Authorization header
const httpClient = axios.create({
  baseURL: "https://localhost:7058/", // Your backend API base URL
});

// Global handler
httpClient.interceptors.request.use(function (config) {
  const token = localStorage.getItem("accessToken");
  let accessToken = "";

  if (token) {
    accessToken = `Bearer ${token}`;
  }
  config.headers.Authorization = accessToken;
  return config;
});

httpClient.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    return Promise.reject(error);
  }
);

export default httpClient;
