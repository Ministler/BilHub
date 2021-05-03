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

    let fd = new FormData();
    fd.append('courseId', courseId);
    fd.append('title', title);
    fd.append('AssignmenDescription', AssignmenDescription);
    fd.append('DueDate', DueDate);
    fd.append('VisibiltyOfSubmission', VisibiltyOfSubmission);
    fd.append('CanBeGradedByStudents', CanBeGradedByStudents);
    fd.append('IsItGraded', IsItGraded);
    var isFormDataEmpty = true;
    for (var p of fd) {
        isFormDataEmpty = false;
        break;
    }
    console.log(isFormDataEmpty);

    return authAxios
        .post(BASE_ASSIGNMENT_URL, fd, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
