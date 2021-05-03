import { authAxios, BASE_SECTION_URL } from '../axiosConfigs';

export const deleteStudentFromSectionRequest = async (email, sectionId) => {
    const url = 'RemoveStudentFromSection';
    const body = {
        sectionId: sectionId,
        email: email,
    };

    return authAxios
        .delete(BASE_SECTION_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
