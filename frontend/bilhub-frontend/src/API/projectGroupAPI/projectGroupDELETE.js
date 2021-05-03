import { authAxios, BASE_PROJECT_GROUP_URL } from '../axiosConfigs';

export const deleteProjectRequest = async (projectGroupId) => {
    const url = 'DeleteProjectGroup?projectGroupId=?' + projectGroupId;

    return authAxios
        .delete(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const deleteSrsGradeRequest = async (projectGroupId) => {
    const url = 'DelteSrsGrade';
    const body = { projectGroupId: projectGroupId };
    return authAxios
        .delete(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
