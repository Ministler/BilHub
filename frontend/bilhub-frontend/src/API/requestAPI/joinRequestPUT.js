import { authAxios, BASE_JOIN_REQUEST_URL } from '../axiosConfigs';

export const putJoinRequest = async (joinRequestId, accept) => {
    const url = joinRequestId + '/' + accept;

    return authAxios
        .delete(BASE_JOIN_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
