import { authAxios, BASE_PEER_GRADE_URL } from '../axiosConfigs';

export const getPeerGradeRequest = async (peerGradeId) => {
    const url = peerGradeId;

    return authAxios
        .get(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getPeerGradeRequestWithoutId = async (projectGroupId, reviewerId, revieweeId) => {
    const url = projectGroupId + '/' + reviewerId + '/' + revieweeId;

    return authAxios
        .get(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
