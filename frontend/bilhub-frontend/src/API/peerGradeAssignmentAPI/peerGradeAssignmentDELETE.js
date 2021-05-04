import { authAxios, BASE_PEER_GRADE_ASSIGNMENT_URL } from '../axiosConfigs';

export const deletePeerGradeAssignmentRequest = async (id) => {
    const url = id;

    return authAxios
        .delete(BASE_PEER_GRADE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
