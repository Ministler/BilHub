import { authAxios, BASE_ASSIGNMENT_URL } from '../axiosConfigs';

export const getAssignmentFileRequest = async (assignmentId) => {
    const url = 'File/' + assignmentId;

    return authAxios
        .get(BASE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getAssignmentRequest = async (assignmentId) => {
    const url = assignmentId;

    return authAxios
        .get(BASE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
