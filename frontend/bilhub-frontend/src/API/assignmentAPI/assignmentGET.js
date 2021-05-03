import { authAxios, BASE_ASSIGNMENT_URL } from '../axiosConfigs';
import FileDownload from 'js-file-download';

export const getAssignmentFileRequest = async (assignmentId) => {
    const url = 'File/' + assignmentId;

    return authAxios
        .get(BASE_ASSIGNMENT_URL + url, { responseType: 'blob' })
        .then((response) => {
            FileDownload(response.data, 'file.pdf');
        })
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

export const getAssignmentFeedsRequest = async () => {
    const url = 'Feeds';

    return authAxios
        .get(BASE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getNotGradedAssignmentRequest = async () => {
    const url = 'NotGraded';

    return authAxios
        .get(BASE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getUpcomingAssignmentFeedsRequest = async () => {
    const url = 'Upcoming';

    return authAxios
        .get(BASE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getAssignmentStatisticsRequest = async (assignmentId) => {
    const url = 'Statistics/' + assignmentId;

    return authAxios
        .get(BASE_ASSIGNMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
