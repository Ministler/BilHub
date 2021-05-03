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

export const putUpdateSRSGradeRequest = async (projectGroupId, srsGrade) => {
    const url = 'UpdateSrsGrade';
    const body = {
        projectGroupId: projectGroupId,
        srsGrade: srsGrade,
    };

    return authAxios
        .put(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
