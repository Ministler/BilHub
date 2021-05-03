import { authAxios, BASE_SECTION_URL } from '../axiosConfigs';

export const postStudentToSectionRequest = async (userId, sectionId) => {
    const url = 'AddStudentToSection?userId=' + userId + '&sectionId=' + sectionId;

    return authAxios
        .post(BASE_SECTION_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
