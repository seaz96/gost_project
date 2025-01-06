import axios from "axios";

export const baseURL = import.meta.env.VITE_APP_API_URL ?? "https://test.gost-storage.ru/api/";

axios.defaults.baseURL = baseURL;

const axiosInstance = axios.create({
	baseURL: baseURL,
});

axiosInstance.interceptors.request.use(
	(config) => {
		const token = localStorage.getItem("jwt_token");
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	},
	(error) => {
		return Promise.reject(error);
	},
);

export { axiosInstance };