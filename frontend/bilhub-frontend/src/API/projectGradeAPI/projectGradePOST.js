import { authAxios, BASE_PROJECT_GRADE_URL } from '../axiosConfigs';

export const postProjectGradeRequest = async (gradedProjectGradeId, maxGrade, grade, comment, file = null) => {
    const url = 'file/' + gradedProjectGradeId + '/' + maxGrade + '/' + grade + '?comment=' + comment;
    const body = {
        file: file,
    };

    return authAxios
        .post(BASE_PROJECT_GRADE_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
