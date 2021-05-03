import { authAxios, BASE_PROJECT_GRADE_URL } from '../axiosConfigs';

import FileDownload from 'js-file-download';

export const getProjectGradeByIdRequest = async (projectGradeId) => {
    const url = 'getById/' + projectGradeId;

    return authAxios
        .get(BASE_PROJECT_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getProjectGradeDownloadByIdRequest = async (gradeId) => {
    const url = 'DownloadById/' + gradeId;

    return authAxios
        .get(BASE_PROJECT_GRADE_URL + url, { responseType: 'blob' })
        .then((response) => {
            FileDownload(response.data, 'file.pdf');
        })
        .catch((error) => {
            throw error;
        });
};

export const getProjectGradeByProjectAndUserIdRequest = async (gradedProjectGroupId, gradingUserId) => {
    const url = 'GetByUsersAndGroup/' + gradedProjectGroupId + '/' + gradingUserId;

    return authAxios
        .get(BASE_PROJECT_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getProjectGradeDownloadByProjectAndUserIdRequest = async (gradedProjectGroupId, gradingUserId) => {
    const url = 'DownloadByUsersAndGroup/' + gradedProjectGroupId + '/' + gradingUserId;

    return authAxios
        .get(BASE_PROJECT_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
