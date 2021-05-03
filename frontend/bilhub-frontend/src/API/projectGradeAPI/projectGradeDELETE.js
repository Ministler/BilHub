import { authAxios, BASE_PROJECT_GRADE_URL } from '../axiosConfigs';

export const deleteProjectGradeRequest = async (projectGradeId) => {
    const url = projectGradeId;

    return authAxios
        .delete(BASE_PROJECT_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
