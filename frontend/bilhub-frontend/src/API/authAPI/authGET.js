import { authAxios, BASE_AUTH_URL } from '../axiosConfigs';

export const checkAuthRequest = async () => {
    const url = 'Check';

    return authAxios
        .get(BASE_AUTH_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getIdByEmailRequest = async (email) => {
    const url = 'IdOfUser?email=' + email;

    return authAxios
        .get(BASE_AUTH_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
