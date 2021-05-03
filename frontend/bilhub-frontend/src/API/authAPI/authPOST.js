import axios from 'axios';

import { BASE_AUTH_URL, authAxios } from '../axiosConfigs';

export const changePassword = async (email, password, newPassword) => {
    const url = 'ChangePassword';
    const body = {
        email: email,
        password: password,
        newPassword: newPassword,
    };

    return authAxios
        .post(BASE_AUTH_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const forgotPasswordRequest = async (email) => {
    const url = 'ForgotPassword';
    const body = {
        email: email,
    };

    return axios
        .post(BASE_AUTH_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const loginRequest = async (email, password) => {
    const url = 'Login';
    const body = {
        email: email,
        password: password,
    };

    return axios
        .post(BASE_AUTH_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const resendRequest = async (email) => {
    const url = 'Resend?email=' + email;

    return axios
        .post(BASE_AUTH_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const registerRequest = async (email, password, name) => {
    const url = 'Register';
    const body = {
        email: email,
        password: password,
        name: name,
    };

    return axios
        .post(BASE_AUTH_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const verifyRequest = async (email, code) => {
    const url = 'Verify';
    const body = {
        email: email,
        code: code,
    };

    return axios
        .post(BASE_AUTH_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
