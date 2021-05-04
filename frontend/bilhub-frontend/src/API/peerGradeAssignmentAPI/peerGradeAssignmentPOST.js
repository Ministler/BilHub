import { authAxios, BASE_PEER_GRADE_ASSIGNMENT_URL } from '../axiosConfigs';

export const postPeerGradeAssignmentRequest = async (courseId, maxGrade, dueDate) => {
    const url = courseId + '/' + maxGrade + '/' + dueDate;

    return authAxios
        .post(BASE_PEER_GRADE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
