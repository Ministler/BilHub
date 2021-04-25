import * as actionTypes from './actionTypes';

import { loginRequest, checkAuthRequest, singupRequest } from '../API';

const loginStart = () => {
    return {
        type: actionTypes.LOGIN_START,
    };
};

const loginSuccess = (token, userId, email, name, userType) => {
    return {
        type: actionTypes.LOGIN_SUCCESS,
        token: token,
        userId: userId,
        email: email,
        name: name,
        userType: userType,
    };
};

const loginFail = (error) => {
    return {
        type: actionTypes.LOGIN_FAIL,
        error: error,
    };
};

const checkAuthSuccess = (token, userId, email, name, userType) => {
    return {
        type: actionTypes.CHECK_AUTH_SUCCESS,
        token: token,
        userId: userId,
        email: email,
        name: name,
        userType: userType,
    };
};

const signupStart = () => {
    return {
        type: actionTypes.SIGNUP_START,
    };
};

const signupSuccess = () => {
    return {
        type: actionTypes.SIGNUP_SUCCESS,
    };
};

const signupFail = (signupError) => {
    return {
        type: actionTypes.SIGNUP_FAIL,
        signupError: signupError,
    };
};

export const resetSignupSucceed = () => {
    return {
        type: actionTypes.RESET_SIGNUP_SUCCEED,
    };
};

export const logout = () => {
    localStorage.removeItem('token');
    return {
        type: actionTypes.LOGOUT,
    };
};

export const login = (email, password) => {
    return (dispatch) => {
        dispatch(loginStart());

        loginRequest(email, password)
            .then((response) => {
                const userData = response.data;
                localStorage.setItem('token', userData.idToken);
                dispatch(
                    loginSuccess(userData.idToken, userData.localId, userData.email, userData.displayName, 'student')
                );
            })
            .catch((error) => {
                dispatch(loginFail(error.response.data.error.message));
            });
    };
};

export const checkAuth = (token) => {
    return (dispatch) => {
        if (!token) {
            dispatch(logout());
            return;
        }
        checkAuthRequest(token)
            .then((response) => {
                const userData = response.data.users[0];
                dispatch(checkAuthSuccess(token, userData.localId, userData.email, userData.displayName, 'student'));
            })
            .catch((error) => {
                alert('Your Authenticantion Expired. Please Login!');
                dispatch(logout());
            });
    };
};

export const signup = (email, password, name) => {
    return (dispatch) => {
        dispatch(signupStart());

        singupRequest(email, password, name)
            .then(() => {
                dispatch(signupSuccess());
            })
            .catch((error) => {
                dispatch(signupFail(error.response.data.error.message));
            });
    };
};
