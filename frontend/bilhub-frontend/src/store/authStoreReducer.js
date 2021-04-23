import * as actionTypes from './actionTypes';
import { updateObject } from '../utils';

const initialState = {
    token: null,
    userId: null,
    name: null,
    userType: null,
    email: null,

    loginError: null,
    loginLoading: false,
    redirectedFromSignup: null,

    signupError: null,
    signupLoading: false,
    signupRequestSucceed: false,

    appLoading: false,
};

const loginStart = (state, action) => {
    return updateObject(state, { loginError: null, loginLoading: true, redirectedFromSignup: false });
};

const loginSuccess = (state, action) => {
    return updateObject(state, {
        token: action.token,
        userId: action.userId,
        email: action.email,
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

const checkAuthStart = (state, action) => {
    return updateObject(state, {
        appLoading: true,
    });
};

const checkAuthSuccess = (state, action) => {
    return updateObject(state, {
        token: action.token,
        userId: action.userId,
        email: action.email,
        name: action.name,
        userType: action.userType,
        checkAuthError: null,
        appLoading: false,
    });
};

const checkAuthFail = (state, action) => {
    return updateObject(state, {
        appLoading: false,
    });
};

const signupStart = (state, action) => {
    return updateObject(state, {
        signUpError: null,
        signupLoading: true,
        signupSuccess: false,
        redirectedFromSignup: false,
    });
};

const signupSuccess = (state, action) => {
    return updateObject(state, {
        signUpError: null,
        signupLoading: false,
        redirectedFromSignup: true,
        signupRequestSucceed: true,
    });
};

const signupFail = (state, action) => {
    return updateObject(state, {
        signupError: action.signupError,
        signupLoading: false,
        signupSuccess: false,
        redirectedFromSignup: false,
    });
};

const logout = (state, action) => {
    return initialState;
};

const resetSignupSucceed = (state, action) => {
    return updateObject(state, {
        signupRequestSucceed: false,
    });
};

export const authReducer = (state = initialState, action) => {
    switch (action.type) {
        case actionTypes.LOGIN_START:
            return loginStart(state, action);
        case actionTypes.LOGIN_SUCCESS:
            return loginSuccess(state, action);
        case actionTypes.LOGIN_FAIL:
            return loginFail(state, action);
        case actionTypes.CHECK_AUTH_START:
            return checkAuthStart(state, action);
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
        case actionTypes.RESET_SIGNUP_SUCCEED:
            return resetSignupSucceed(state, action);
        default:
            return state;
    }
};
