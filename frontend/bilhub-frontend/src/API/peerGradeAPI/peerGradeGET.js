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

export const getPeerGradeRequestWithReviewer = async (projectGroupId, reviewerId) => {
    const url = projectGroupId + '/' + reviewerId;

    return authAxios
        .get(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getPeerGradeRequestWithReviewee = async (projectGroupId, revieweeId) => {
    const url = projectGroupId + '/' + revieweeId;

    return authAxios
        .get(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
