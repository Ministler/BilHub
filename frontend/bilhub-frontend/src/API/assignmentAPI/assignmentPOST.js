import { authAxios, BASE_ASSIGNMENT_URL } from '../axiosConfigs';

export const postAssignmentFileRequest = async (assignmentId, file) => {
    const url = 'File/' + assignmentId;
    const body = {
        file: file,
    };

    return authAxios
        .post(BASE_ASSIGNMENT_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postAssignmentRequest = async (
    file,
    courseId,
    title,
    AssignmenDescription,
    DueDate,
    VisibiltyOfSubmission,
    CanBeGradedByStudents,
    IsItGraded
) => {
    const url = assignmentId;
    const body = {
        file: file,
        courseId: courseId,
        title: title,
        AssignmenDescription: AssignmenDescription,
        DueDate: DueDate,
        VisibiltyOfSubmission: VisibiltyOfSubmission,
        CanBeGradedByStudents: CanBeGradedByStudents,
        IsItGraded: IsItGraded,
    };

    return authAxios
        .post(BASE_ASSIGNMENT_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
