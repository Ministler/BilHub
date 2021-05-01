import { authAxios, BASE_ASSIGNMENT_URL } from '../axiosConfigs';

export const deleteAssignmentFileRequest = async (assignmentId) => {
    const url = 'File/' + assignmentId;

    return authAxios
        .delete(BASE_ASSIGNMENT_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const deleteAssignmentRequest = async (assignmentId) => {
    const url = assignmentId;

    return authAxios
        .delete(BASE_ASSIGNMENT_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
