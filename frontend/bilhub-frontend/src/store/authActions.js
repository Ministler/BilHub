import * as actionTypes from './actionTypes';

import { loginRequest, checkAuthRequest, singupRequest } from '../API';

const loginStart = () => {
    return {
        type: actionTypes.LOGIN_START,
    };
};

const loginSuccess = (token, userId, name, userType) => {
    return {
        type: actionTypes.LOGIN_SUCCESS,
        token: token,
        userId: userId,
        name: name,
        userType: userType,
    };
};

const loginFail = (loginError) => {
    return {
        type: actionTypes.LOGIN_FAIL,
        loginError: loginError,
    };
};

const checkAuthSuccess = (token, userId, name, userType) => {
    return {
        type: actionTypes.CHECK_AUTH_SUCCESS,
        token: token,
        userId: userId,
        name: name,
        userType: userType,
    };
};

const checkAuthFail = (checkAuthError) => {
    return {
        type: actionTypes.CHECK_AUTH_FAIL,
        checkAuthError: checkAuthError,
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

export const login = (email, password) => {
    return (dispatch) => {
        dispatch(loginStart());

        loginRequest(email, password)
            .then((response) => {
                console.log(response.data);
                localStorage.setItem('token', response.data.idToken);
                //dispatch(loginSuccess(response.data.idToken, response.data.localId));
            })
            .catch((error) => {
                console.log(error.data);
                dispatch(loginFail(error.response.data.error));
            });
    };
};

export const checkAuth = () => {
    return (dispatch) => {
        const token = localStorage.getItem('token');
        if (!token) {
            dispatch(checkAuthFail());
            dispatch(logout());
            return;
        }

        checkAuthRequest(token)
            .then((response) => {
                console.log(response);
            })
            .catch((error) => {
                dispatch(checkAuthFail(error.response.data.error));
            });
    };
};

export const signup = (firstName, lastName, email, password) => {
    return (dispatch) => {
        dispatch(signupStart());

        singupRequest(firstName, lastName, email, password)
            .then(() => {
                dispatch(signupSuccess());
            })
            .catch((error) => {
                dispatch(signupFail(error.response.data.error));
            });
    };
};

export const logout = (dispatch) => {
    localStorage.removeItem('token');
    dispatch({
        type: actionTypes.LOGOUT,
    });
};
