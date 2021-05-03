import { authAxios, BASE_JOIN_REQUEST_URL } from '../axiosConfigs';

export const postJoinRequest = async (requestedGroupId, description) => {
    const url = requestedGroupId;
    const body = {
        description: description,
    };

    return authAxios
        .post(BASE_JOIN_REQUEST_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
