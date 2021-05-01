import axios from 'axios';

export const authAxios = axios.create({
    baseURL: '',
    headers: { Authorization: 'Bearer ' },
});

export const unauthAxios = axios.create({
    baseURL: '',
});

authAxios.interceptors.request.use(
    (request) => {
        return request;
    },
    (error) => {
        return Promise.reject(error);
    }
);

authAxios.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        return Promise.reject(error);
    }
);
