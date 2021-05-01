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
