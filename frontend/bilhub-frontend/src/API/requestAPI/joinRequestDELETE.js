import { authAxios, BASE_JOIN_REQUEST_URL } from '../axiosConfigs';

export const deleteJoinRequest = async (joinRequestId) => {
    const url = joinRequestId;

    return authAxios
        .delete(BASE_JOIN_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
