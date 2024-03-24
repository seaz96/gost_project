import axios from "axios";

axios.defaults.baseURL = 'https://gost-storage.ru/api'

const axiosInstance = axios.create({
    baseURL: 'https://gost-storage.ru/api/'
});

axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('jwt_token')}`;

export {axiosInstance}