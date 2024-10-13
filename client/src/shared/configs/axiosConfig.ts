import axios from "axios";

axios.defaults.baseURL = process.env.REACT_APP_API_URL ?? 'https://test.gost-storage.ru/api/';

const axiosInstance = axios.create({
    baseURL: `${process.env.REACT_APP_API_URL ?? 'https://test.gost-storage.ru/api'}/`
});

axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('jwt_token')}`;

export {axiosInstance}