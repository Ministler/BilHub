import { authAxios, BASE_COURSE_URL } from '../axiosConfigs';

export const deleteInstructorFromCourseRequest = async () => {
    const url = 'Course/RemoveInstructorFromCourse';

    return authAxios
        .delete(BASE_COURSE_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
