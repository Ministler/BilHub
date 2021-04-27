import * as actionTypes from './actionTypes';
import { updateObject } from '../utils';

const initialState = {
    token: null,
    userId: null,
    name: null,
    userType: null,
    email: null,

    appLoading: true,
};

const authSuccess = (state, action) => {
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

const logout = (state, action) => {
    return { ...initialState, appLoading: false };
};

export const authReducer = (state = initialState, action) => {
    switch (action.type) {
        case actionTypes.AUTH_SUCCESS:
            return authSuccess(state, action);
        case actionTypes.LOGOUT:
            return logout(state, action);
        default:
            return state;
    }
};
