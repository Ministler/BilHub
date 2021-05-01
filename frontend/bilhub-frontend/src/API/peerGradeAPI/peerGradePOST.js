import { authAxios, BASE_PEER_GRADE_URL } from '../axiosConfigs';

export const postPeerGradeRequest = async (projectGroupId, revieweeId, maxGrade, grade) => {
    const url = projectGroupId + '/' + revieweeId + '/' + maxGrade + '/' + grade;

    return authAxios
        .post(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
