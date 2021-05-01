import { authAxios, BASE_COMMENT_URL } from '../axiosConfigs';

export const getCommentFileZipRequest = async (commentId) => {
    const url = 'File/Zip/' + commentId;

    return authAxios
        .get(BASE_COMMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getCommentFileRequest = async (commentId) => {
    const url = 'File/' + commentId;

    return authAxios
        .get(BASE_COMMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getCommentRequest = async (commentId) => {
    const url = commentId;

    return authAxios
        .get(BASE_COMMENT_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
