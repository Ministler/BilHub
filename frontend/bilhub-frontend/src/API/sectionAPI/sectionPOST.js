import { authAxios, BASE_SECTION_URL } from '../axiosConfigs';

export const postStudentToSectionRequest = async (email, sectionId) => {
    const url = 'AddStudentToSection';
    const body = {
        email: email,
    };

    return authAxios
        .delete(BASE_SECTION_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
