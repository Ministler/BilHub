import { authAxios, BASE_PEER_GRADE_URL } from '../axiosConfigs';

export const postPeerGradeRequest = async (projectGroupId, revieweeId, grade, comment) => {
    const url = projectGroupId + '/' + revieweeId + '/' + grade + '?comment=' + comment;

    return authAxios
        .post(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
