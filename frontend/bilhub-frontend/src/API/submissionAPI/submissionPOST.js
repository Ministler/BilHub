import { authAxios, BASE_SUBMISSION_URL } from '../axiosConfigs';

export const postSubmissionRequest = async (file, Description, AffiliatedAssignmentId) => {
    const body = {
        file: file,
        Description: Description,
        AffiliatedAssignmentId: AffiliatedAssignmentId,
    };

    return authAxios
        .post(BASE_SUBMISSION_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postSubmissionFileRequest = async (file, submissionId) => {
    const url = submissionId;
    const body = {
        file: file,
    };

    return authAxios
        .post(BASE_SUBMISSION_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postSubmissionSrsGradeRequest = async (submissionId, srsGrade) => {
    const url = 'SrsGrade/' + submissionId;
    const body = {
        srsGrade: srsGrade,
    };

    return authAxios
        .post(BASE_SUBMISSION_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
