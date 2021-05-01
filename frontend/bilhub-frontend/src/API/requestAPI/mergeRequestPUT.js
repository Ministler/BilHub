import { authAxios, BASE_MERGE_REQUEST_URL } from '../axiosConfigs';

export const putMergeRequest = async (mergeRequestId, accept) => {
    const url = mergeRequestId + '/' + accept;

    return authAxios
        .delete(BASE_MERGE_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
