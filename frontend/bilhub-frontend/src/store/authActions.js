import * as actionTypes from './actionTypes';

export const authSuccess = (token, userId, email, name, userType) => {
    localStorage.setItem('token', token);
    return {
        type: actionTypes.AUTH_SUCCESS,
        token: token,
        userId: userId,
        email: email,
        name: name,
        userType: userType,
    };
};

export const logout = () => {
    localStorage.removeItem('token');
    return {
        type: actionTypes.LOGOUT,
    };
};
