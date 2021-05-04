import { authAxios, BASE_PEER_GRADE_ASSIGNMENT_URL } from '../axiosConfigs';

export const getPeerGradeAssignmentRequest = async (courseId) => {
    const url = courseId;

    return authAxios
        .get(BASE_PEER_GRADE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
