import { authAxios, BASE_SECTION_URL } from '../axiosConfigs';

export const getSectionRequest = async (sectionId) => {
    const url = sectionId;

    return authAxios
        .get(BASE_SECTION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
