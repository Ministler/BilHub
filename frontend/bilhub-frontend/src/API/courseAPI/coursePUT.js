import { authAxios, BASE_COURSE_URL } from '../axiosConfigs';

export const putCourseRequest = async (
    id,
    name,
    courseSemester,
    year,
    courseInformation,
    courseDescription,
    minGroupSize,
    maxGroupSize
) => {
    const body = {
        id: id,
        name: name,
        courseSemester: courseSemester,
        year: year,
        courseInformation: courseInformation,
        courseDescription: courseDescription,
        minGroupSize: minGroupSize,
        maxGroupSize: maxGroupSize,
    };

    return authAxios
        .put(BASE_COURSE_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
