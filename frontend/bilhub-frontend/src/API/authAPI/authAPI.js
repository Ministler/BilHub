import axios from 'axios';

export const checkAuthRequest = async (token) => {
    const url = 'https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=' + token;
    return axios.post(url);
};

export const loginRequest = async (email, password) => {
    const authData = {
        email: email,
        password: password,
        returnSecureToken: true,
    };
    const url = 'https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyDqcfKSD_bCTD9EeMo40bNFyZRBB1kucBc';

    return axios.post(url, authData);
};

export const singupRequest = async (firstName, lastName, email, password) => {
    const authData = {
        email: email,
        password: password,
        firstName: firstName,
        lastName: lastName,
    };
    const url =
        'https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyDqcfKSD_bCTD9EeMo40bNFyZRBB1kucBc';

    return axios.post(url, authData);
};
