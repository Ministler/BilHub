import { authAxios, BASE_SUBMISSION_URL } from '../axiosConfigs';

export const postSubmissionRequest = async (file, Description, submissionId) => {
    console.log(Description, submissionId, file);
    const fd = new FormData();
    fd.append('file', file);
    fd.append('updateSubmissionDto.SubmissionId', submissionId);
    fd.append('updateSubmissionDto.Description', Description);
    return authAxios
        .put(BASE_SUBMISSION_URL, fd, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postSubmissionFileRequest = async (file, submissionId) => {
    const url = 'File/' + submissionId;

    const fd = FormData();
    fd.append('file', file);

    return authAxios
        .put(BASE_SUBMISSION_URL + url, fd, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postSubmissionSrsGradeRequest = async (submissionId, srsGrade) => {
    const url = 'SrsGrade/' + submissionId;
    const body = {
        srsGrade: srsGrade,
    };

    return authAxios
        .post(BASE_SUBMISSION_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
