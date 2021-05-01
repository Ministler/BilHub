import { authAxios, BASE_COMMENT_URL } from '../axiosConfigs';

export const putCommentRequest = async (File, CommentId, CommentText, MaxGrade, Grade) => {
    const body = {
        File: File,
        CommentId: CommentId,
        CommentText: CommentText,
        MaxGrade: MaxGrade,
        Grade: Grade,
    };

    return authAxios
        .put(BASE_COMMENT_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
