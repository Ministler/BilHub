import { BASE_AUTH_URL, authAxios } from '../axiosConfigs';

export const updateProfile = async (email, name, profileInfo, darkModeStatus) => {
    const url = 'UpdateProfileInfo';
    const body = {
        email: email,
        name: name,
        profileInfo: profileInfo,
        darkModeStatus: darkModeStatus,
    };

    return authAxios
        .put(BASE_AUTH_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
