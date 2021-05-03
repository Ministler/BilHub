import { authAxios, BASE_PROJECT_GRADE_URL } from '../axiosConfigs';

export const postProjectGradeRequest = async (gradedProjectGradeId, maxGrade, grade, comment, file) => {
    const url = 'file/' + gradedProjectGradeId + '/' + maxGrade + '/' + grade + '?comment=' + comment;
    const body = {
        file: file,
    };
    console.log(file);
    const fd = new FormData();
    fd.append('file', file);

    return authAxios
        .post(BASE_PROJECT_GRADE_URL + url, fd, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
