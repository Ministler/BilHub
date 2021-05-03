import { authAxios, BASE_MERGE_REQUEST_URL } from '../axiosConfigs';

export const getMergeRequest = async (mergeRequestId) => {
    const url = mergeRequestId;

    return authAxios
        .get(BASE_MERGE_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getIncomingMergeRequest = async () => {
    const url = 'IncomingMergeRequestsOfUser';

    return authAxios
        .get(BASE_MERGE_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getOutgoingMergeRequest = async () => {
    const url = 'OutgoingMergeRequestsOfUser';

    return authAxios
        .get(BASE_MERGE_REQUEST_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
