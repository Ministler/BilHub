import axios from 'axios';

// General Settings
axios.defaults.baseURL = 'http://00679148e8f2.ngrok.io/';

// Authorized Requests
export const authAxios = axios.create({
    headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
});

// Unauthorized Requests
export const unauthAxios = axios.create({
    baseURL: 'Auth/',
});

// Authorized Requests Interceptors
authAxios.interceptors.request.use(
    (request) => {
        return request;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Authorized Responses Interceptors
authAxios.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Base URLS
export const BASE_ASSIGNMENT_URL = 'Assignment/';
export const BASE_COURSE_URL = 'Course/';
export const BASE_COMMENT_URL = 'Comment/';
export const BASE_SECTION_URL = 'Section/';
