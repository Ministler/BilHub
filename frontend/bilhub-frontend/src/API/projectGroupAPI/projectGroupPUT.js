import { authAxios, BASE_PROJECT_GROUP_URL } from '../axiosConfigs';

export const putProjectGroupInformationRequest = async (id, projectInformation) => {
    const body = {
        id: id,
        projectInformation: projectInformation,
    };

    return authAxios
        .put(BASE_PROJECT_GROUP_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
