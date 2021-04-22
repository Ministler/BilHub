import * as actionTypes from './actionTypes';
import { updateObject } from '../utils';

const initialState = {
    token: null,
    userId: null,
    name: null,
    userType: null,
    loginError: null,
    signupError: null,
    signupSuccess: null,
    checkAuthError: null,
    loginLoading: false,
    signupLoading: false,
};

const loginStart = (state, action) => {
    return updateObject(state, { loginError: null, loginLoading: true, signupSuccess: null });
};

const loginSuccess = (state, action) => {
    return updateObject(state, {
        token: action.idToken,
        userId: action.userId,
        name: action.name,
        userType: action.userType,
        loginError: null,
        loginLoading: false,
    });
};

const loginFail = (state, action) => {
    return updateObject(state, {
        loginError: action.error,
        loginLoading: false,
    });
};

const checkAuthSuccess = (state, action) => {
    return updateObject(state, {
        token: action.token,
        userId: action.userId,
        name: action.name,
        userType: action.userType,
        checkAuthError: null,
    });
};

const checkAuthFail = (state, checkAuthError) => {
    return updateObject(state, {
        checkAuthError: checkAuthError,
    });
};

const signupStart = (state, action) => {
    return updateObject(state, { signUpError: null, signupLoading: true, signupSuccess: null });
};

const signupSuccess = (state, action) => {
    return updateObject(state, {
        signUpError: null,
        signupLoading: false,
        signupSuccess: true,
    });
};

const signupFail = (state, action) => {
    return updateObject(state, {
        signupError: action.signupError,
        signupLoading: false,
        signupSuccess: false,
    });
};

const logout = (state, action) => {
    return initialState;
};

export const authReducer = (state = initialState, action) => {
    switch (action.type) {
        case actionTypes.LOGIN_START:
            return loginStart(state, action);
        case actionTypes.LOGIN_SUCCESS:
            return loginSuccess(state, action);
        case actionTypes.LOGIN_FAIL:
            return loginFail(state, action);
        case actionTypes.CHECK_AUTH_SUCCESS:
            return checkAuthSuccess(state, action);
        case actionTypes.CHECK_AUTH_FAIL:
            return checkAuthFail(state, action);
        case actionTypes.SIGNUP_START:
            return signupStart(state, action);
        case actionTypes.SIGNUP_SUCCESS:
            return signupSuccess(state, action);
        case actionTypes.SIGNUP_FAIL:
            return signupFail(state, action);
        case actionTypes.LOGOUT:
            return logout(state, action);
        default:
            return state;
    }
};
