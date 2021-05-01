import { unauthAxios } from '../axiosConfigs';

export const checkAuthRequest = async (token) => {
    const url = 'Check';
    const body = {
        token: token,
    };

    return unauthAxios
        .post(url, body)
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

    return unauthAxios
        .post(url, body)
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

    return unauthAxios
        .post(url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const resendRequest = async (email) => {
    const url = 'Resend';
    const body = {
        email: email,
    };

    return unauthAxios
        .post(url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const registerRequest = async (email, password, name) => {
    const url = '/Register';
    const body = {
        email: email,
        password: password,
        displayName: name,
    };

    return unauthAxios
        .post(url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const verifyRequest = async (email, code) => {
    const url = '/Verify';
    const body = {
        email: email,
        code: code,
    };

    return unauthAxios
        .post(url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const changePassword = async (email, password, newPassword) => {
    const url = '/ChangePassword';
    const body = {
        email: email,
        code: code,
    };

    return unauthAxios
        .post(url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
