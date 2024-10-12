import axios from "axios";

axios.defaults.baseURL = process.env.REACT_APP_API_URL

const axiosInstance = axios.create({
    baseURL: `${process.env.REACT_APP_API_URL}/`
});

axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('jwt_token')}`;

export {axiosInstance}