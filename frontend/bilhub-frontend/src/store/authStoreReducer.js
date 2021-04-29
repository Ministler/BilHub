import * as actionTypes from './actionTypes';
import { updateObject } from '../utils';

const initialState = {
    token: null,
    userId: null,
    name: null,
    userType: null,
    email: null,
    darkMode: false,

    appLoading: true,
};

const authSuccess = (state, action) => {
    return updateObject(state, {
        token: action.token,
        userId: action.userId,
        name: action.name,
        userType: action.userType,
        email: action.email,
        darkMode: action.darkMode,
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
