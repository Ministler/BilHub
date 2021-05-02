import * as actionTypes from './actionTypes';

export const authSuccess = (token, userId, email, name, userType, darkMode) => {
    localStorage.setItem('token', token);
    return {
        type: actionTypes.AUTH_SUCCESS,
        userId: userId,
        name: name,
        userType: userType,
        email: email,
        darkMode: darkMode,
    };
};

export const logout = () => {
    localStorage.removeItem('token');
    return {
        type: actionTypes.LOGOUT,
    };
};
