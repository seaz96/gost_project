import axios from "axios";

axios.defaults.baseURL = import.meta.env.VITE_APP_API_URL ?? "https://test.gost-storage.ru/api/";

const axiosInstance = axios.create({
	baseURL: `${import.meta.env.VITE_APP_API_URL ?? "https://test.gost-storage.ru/api"}/`,
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