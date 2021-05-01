import { authAxios, BASE_PROJECT_GRADE_URL } from '../axiosConfigs';

export const putProjectGradeRequest = async (projectGradeId, maxGrade, grade) => {
    const url = 'file/' + projectGradeId + '/' + maxGrade + '/' + grade;

    return authAxios
        .put(BASE_PROJECT_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
