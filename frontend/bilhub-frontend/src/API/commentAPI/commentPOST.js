import { authAxios, BASE_COMMENT_URL } from '../axiosConfigs';

export const postCommentFileRequest = async (File, commentId) => {
    const url = commentId;
    const body = {
        File: File,
    };

    return authAxios
        .post(BASE_COMMENT_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postCommentRequest = async (File, CommentedSubmissionId, CommentText, MaxGrade, Grade) => {
    let fd = new FormData();
    fd.append('File', File);
    fd.append('AddCommentDto.CommentedSubmissionId', CommentedSubmissionId);
    fd.append('AddCommentDto.CommentText', CommentText);
    fd.append('AddCommentDto.MaxGrade', MaxGrade);
    fd.append('AddCommentDto.Grade', Grade);

    return authAxios
        .post(BASE_COMMENT_URL, fd, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
