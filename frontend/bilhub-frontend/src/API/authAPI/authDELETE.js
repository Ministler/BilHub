import axios from 'axios';

export const checkAuthRequest = async (token) => {
    const url = 'https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=AIzaSyDqcfKSD_bCTD9EeMo40bNFyZRBB1kucBc';
    return axios
        .post(url, { idToken: token })
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const loginRequest = async (email, password) => {
    const authData = {
        email: email,
        password: password,
        returnSecureToken: true,
    };
    const url =
        'https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyDqcfKSD_bCTD9EeMo40bNFyZRBB1kucBc';

    return axios
        .post(url, authData)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const singupRequest = async (email, password, name) => {
    const authData = {
        email: email,
        password: password,
        displayName: name,
    };
    const url = 'https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyDqcfKSD_bCTD9EeMo40bNFyZRBB1kucBc';

    return axios
        .post(url, authData)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
