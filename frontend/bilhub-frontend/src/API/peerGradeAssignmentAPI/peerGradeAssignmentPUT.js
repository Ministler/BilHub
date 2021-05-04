import { authAxios, BASE_PEER_GRADE_ASSIGNMENT_URL } from '../axiosConfigs';

export const putPeerGradeAssignmentRequest = async (id, maxGrade, dueDate) => {
    const url = id + '/' + maxGrade + '/' + dueDate;

    return authAxios
        .put(BASE_PEER_GRADE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
