import { authAxios, BASE_PROJECT_GRADE_URL } from '../axiosConfigs';

export const putProjectGradeRequest = async (projectGradeId, maxGrade, grade, comment) => {
    const url = 'file/' + projectGradeId + '/' + maxGrade + '/' + grade + '?comment=' + comment;

    return authAxios
        .put(BASE_PROJECT_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
