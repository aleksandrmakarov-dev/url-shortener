import axios from "axios";

const config = {
  baseURL: "/api/v1",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
};

let axiosToken: string | undefined = "";

export const setAuthorizationToken = (token?: string) => {
  axiosToken = token;
};

const instance = axios.create(config);

instance.interceptors.request.use((config) => {
  if (axiosToken && !config.headers["Authorization"]) {
    config.headers["Authorization"] = `Bearer ${axiosToken}`;
  }

  return config;
});

export default instance;
