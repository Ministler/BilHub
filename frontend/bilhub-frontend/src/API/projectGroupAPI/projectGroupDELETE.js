import { authAxios, BASE_PROJECT_GROUP_URL } from '../axiosConfigs';

export const deleteProjectRequest = async (projectGroupId) => {
    const url = 'DeleteProjectGroup';
    const body = {
        projectGroupId: projectGroupId,
    };

    return authAxios
        .delete(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
