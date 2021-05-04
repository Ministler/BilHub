import { authAxios, BASE_COURSE_URL } from '../axiosConfigs';

export const putCourseRequest = async (
    id,
    name,
    courseSemester,
    year,
    courseInformation,
    courseDescription,
    lockDate,
    minGroupSize,
    maxGroupSize,
    isActive,
    isLocked
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
        lockDate: lockDate,
        isActive: isActive,
        isLocked: isLocked,
    };

    return authAxios
        .put(BASE_COURSE_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
