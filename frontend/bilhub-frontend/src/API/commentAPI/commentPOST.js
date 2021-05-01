import { authAxios, BASE_COMMENT_URL } from '../axiosConfigs';

export const postCommentRequest = async (File, commentId) => {
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
    const body = {
        File: File,
        CommentedSubmissionId: CommentedSubmissionId,
        CommentText: CommentText,
        MaxGrade: MaxGrade,
        Grade: Grade,
    };

    return authAxios
        .post(BASE_COMMENT_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
