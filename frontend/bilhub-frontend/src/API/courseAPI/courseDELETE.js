import { authAxios, BASE_COURSE_URL } from '../axiosConfigs';

export const deleteInstructorFromCourseRequest = async () => {
    const url = 'Course/RemoveInstructorFromCourse';

    return authAxios
        .delete(BASE_COURSE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const deleteCourseRequest = async (courseId) => {
    const url = courseId;

    return authAxios
        .delete(BASE_COURSE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
