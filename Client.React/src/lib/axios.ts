import axios from "axios";

const config = {
  baseURL: "/api",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
};

const instance = axios.create(config);

export default instance;
