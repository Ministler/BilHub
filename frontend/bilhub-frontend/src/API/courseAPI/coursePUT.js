import { authAxios, BASE_COURSE_URL } from '../axiosConfigs';

export const putCourseRequest = async (
    id,
    name,
    courseSemester,
    year,
    courseInformation,
    lockDate,
    minGroupSize,
    maxGroupSize,
    courseDescription
) => {
    const body = {
        id: id,
        name: name,
        courseSemester: courseSemester,
        year: year,
        courseInformation: courseInformation,
        lockDate: lockDate,
        minGroupSize: minGroupSize,
        maxGroupSize: maxGroupSize,
        courseDescription: courseDescription,
    };

    return authAxios
        .put(BASE_COURSE_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
