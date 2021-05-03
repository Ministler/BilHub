import { authAxios, BASE_MERGE_REQUEST_URL } from '../axiosConfigs';

export const deleteMergeRequest = async (mergeRequestId) => {
    const url = mergeRequestId;

    return authAxios
        .delete(BASE_MERGE_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
