import { authAxios, BASE_PROJECT_GRADE_URL } from '../axiosConfigs';

export const postProjectGradeRequest = async (gradedProjectGradeId, maxGrade, grade, file) => {
    const url = 'file/' + gradedProjectGradeId + '/' + maxGrade + '/' + grade;
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
