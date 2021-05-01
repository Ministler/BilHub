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
export const BASE_MERGE_REQUEST_URL = 'MergeRequest/';
export const BASE_JOIN_REQUEST_URL = 'JoinRequest/';
export const BASE_PEER_GRADE_URL = 'PeerGrade/';
export const BASE_PROJECT_GRADE_URL = 'ProjectGrade/';
export const BASE_PROJECT_GROUP_URL = 'ProjectGroup/';
export const BASE_SUBMISSION_URL = 'Submission/';
