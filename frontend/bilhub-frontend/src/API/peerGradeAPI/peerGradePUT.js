import { authAxios, BASE_PEER_GRADE_URL } from '../axiosConfigs';

export const putPeerGradeRequest = async (peerGradeId, grade, comment) => {
    const url = peerGradeId + '/' + grade + '?comment=' + comment;

    return authAxios
        .put(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
