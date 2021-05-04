import axios from 'axios';

import { store } from '../index';
import { logout } from '../store';

// General Settings
axios.defaults.baseURL = 'https://5c7746ce1640.ngrok.io/';

// Authorized Requests
export const authAxios = axios.create();

// Authorized Requests Interceptors
authAxios.interceptors.request.use(
    (request) => {
        request.headers['Authorization'] = 'Bearer ' + localStorage.getItem('token');
        console.log('@@@@@@@@@@@@@@@@@@@@@@@ REQUEST @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@');
        console.log(request);
        console.log('@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@');
        return request;
    },
    (error) => {
        console.log('@@@@@@@@@@@@@@@@@@@@ RESPONSE ERROR @@@@@@@@@@@@@@@@@@@@@@@@@@@@');
        console.log(error.response);
        console.log('@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@');
        return Promise.reject(error);
    }
);

// Authorized Responses Interceptors
authAxios.interceptors.response.use(
    (response) => {
        console.log('@@@@@@@@@@@@@@@@@@@@@@@ RESPONSE @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@');
        console.log(response.data);
        console.log('@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@');
        return response;
    },
    (error) => {
        if (error.response?.status === 401) {
            store.dispatch(logout());
        }
        console.log('@@@@@@@@@@@@@@@@@@@@@ RESPONSE ERROR @@@@@@@@@@@@@@@@@@@@@@@@@@@');
        console.log(error.response);
        console.log('@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@');
        return Promise.reject(error);
    }
);

// Base URLS
export const BASE_AUTH_URL = 'Auth/';
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
export const BASE_PEER_GRADE_ASSIGNMENT_URL = 'PeerGradeAssignment/';
