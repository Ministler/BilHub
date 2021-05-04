import { authAxios, BASE_SUBMISSION_URL } from '../axiosConfigs';
import FileDownload from 'js-file-download';

export const getUngradedSubmissionFileRequest = async (courseId) => {
    const url = 'File/Ungraded/' + courseId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSubmissionFileOfSectionRequest = async (courseId, sectionId, assignmentId) => {
    const url = 'File/' + courseId + '/' + sectionId + '/' + assignmentId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSubmissionRequest = async (submissionId) => {
    const url = submissionId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getNewCommentsRequest = async (submissionId) => {
    const url = 'Feedbacks';

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSubmissionInstructorCommentsRequest = async (submissionId) => {
    const url = 'InstructorComments/' + submissionId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSubmissionTACommentsRequest = async (submissionId) => {
    const url = 'TAComments/' + submissionId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSubmissionStudentCommentsRequest = async (submissionId) => {
    const url = 'StudentComments/' + submissionId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSubmissionSrsGradeRequest = async (submissionId, graderId) => {
    const url = 'Grade/' + submissionId + '/' + graderId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSubmissionFileRequest = async (submissionId, fileName) => {
    const url = 'File/' + submissionId;

    return authAxios
        .get(BASE_SUBMISSION_URL + url, { responseType: 'blob' })
        .then((response) => {
            FileDownload(response.data, fileName ? fileName : 'file.pdf');
        })
        .catch((error) => {
            throw error;
        });
};
