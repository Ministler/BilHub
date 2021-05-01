import { authAxios, BASE_PEER_GRADE_URL } from '../axiosConfigs';

export const putPeerGradeRequest = async (peerGradeId, maxGrade, grade) => {
    const url = peerGradeId + '/' + maxGrade + '/' + grade;

    return authAxios
        .put(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
