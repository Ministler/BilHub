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
        CourseId: courseId,
        Title: title,
        AssignmenDescription: AssignmenDescription,
        DueDate: DueDate,
        VisibiltyOfSubmission: VisibiltyOfSubmission,
        CanBeGradedByStudents: CanBeGradedByStudents,
        IsItGraded: IsItGraded,
    };

    let fd = new FormData();
    fd.append('addAssignmentDto.CourseId', courseId);
    fd.append('addAssignmentDto.Title', title);
    fd.append('addAssignmentDto.AssignmenDescription', AssignmenDescription);
    fd.append('addAssignmentDto.DueDate', DueDate);
    fd.append('addAssignmentDto.VisibiltyOfSubmission', VisibiltyOfSubmission);
    fd.append('addAssignmentDto.CanBeGradedByStudents', CanBeGradedByStudents);
    fd.append('addAssignmentDto.IsItGraded', IsItGraded);
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
