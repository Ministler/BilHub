import * as actionTypes from './actionTypes';

export const authSuccess = (token, userId, email, name, userType, darkMode) => {
    console.log(token);
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
    console.log('asd');
    localStorage.removeItem('token');
    return {
        type: actionTypes.LOGOUT,
    };
};
