import { authAxios, BASE_ASSIGNMENT_URL } from '../axiosConfigs';

export const putAssignmentRequest = async (
    file,
    AssignmentId,
    courseId,
    title,
    AssignmenDescription,
    DueDate,
    VisibiltyOfSubmission,
    CanBeGradedByStudents,
    IsItGraded
) => {
    const body = {
        file: file,
        AssignmentId: AssignmentId,
        courseId: courseId,
        title: title,
        AssignmenDescription: AssignmenDescription,
        DueDate: DueDate,
        VisibiltyOfSubmission: VisibiltyOfSubmission,
        CanBeGradedByStudents: CanBeGradedByStudents,
        IsItGraded: IsItGraded,
    };

    return authAxios
        .put(BASE_ASSIGNMENT_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
