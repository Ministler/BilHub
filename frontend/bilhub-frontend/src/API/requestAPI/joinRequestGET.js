import { authAxios, BASE_JOIN_REQUEST_URL } from '../axiosConfigs';

export const getJoinRequest = async (joinRequestId) => {
    const url = joinRequestId;

    return authAxios
        .get(BASE_JOIN_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
