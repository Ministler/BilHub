import { authAxios, BASE_COURSE_URL } from '../axiosConfigs';

export const postCourseRequest = async (
    name,
    courseSemester,
    year,
    courseInformation,
    numberOfSections,
    isSectionless,
    lockDate,
    minGroupSize,
    maxGroupSize
) => {
    const body = {
        name: name,
        courseSemester: courseSemester,
        year: year,
        courseInformation: courseInformation,
        numberOfSections: numberOfSections,
        isSectionless: isSectionless,
        lockDate: lockDate,
        minGroupSize: minGroupSize,
        maxGroupSize: maxGroupSize,
    };

    return authAxios
        .post(BASE_COURSE_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postCourseInstructorRequest = async (email, courseId) => {
    const url = 'AddInstructorToCourse';
    const body = {
        email: email,
        courseId: courseId,
    };

    return authAxios
        .post(BASE_COURSE_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
