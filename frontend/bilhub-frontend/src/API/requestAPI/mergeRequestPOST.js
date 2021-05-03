import { authAxios, BASE_MERGE_REQUEST_URL } from '../axiosConfigs';

export const postMergeRequest = async (receiverGroupId, description) => {
    const url = receiverGroupId;
    const body = {
        description: description,
    };

    return authAxios
        .post(BASE_MERGE_REQUEST_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
