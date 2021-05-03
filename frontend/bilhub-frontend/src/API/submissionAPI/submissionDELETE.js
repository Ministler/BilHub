import { authAxios, BASE_SUBMISSION_URL } from '../axiosConfigs';

export const deleteSubmissionRequest = async (submissionId) => {
    const url = submissionId;

    return authAxios
        .delete(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const deleteSubmissionFileRequest = async (submissionId) => {
    const url = 'File/' + submissionId;

    return authAxios
        .delete(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const deleteSubmissionSrsGradeRequest = async (submissionId) => {
    const url = 'SrsGrade/' + submissionId;

    return authAxios
        .delete(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
