import { authAxios, BASE_COMMENT_URL } from '../axiosConfigs';

export const deleteCommentFileRequest = async (commentId) => {
    const url = 'File/' + commentId;

    return authAxios
        .delete(BASE_COMMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const deleteCommentRequest = async (commentId) => {
    const url = commentId;

    return authAxios
        .delete(BASE_COMMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
