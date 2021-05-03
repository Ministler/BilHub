import { authAxios, BASE_SUBMISSION_URL } from '../axiosConfigs';

export const putSubmissionRequest = async (file, Description, SubmissionId) => {
    const body = {
        file: file,
        Description: Description,
        SubmissionId: SubmissionId,
    };

    return authAxios
        .put(BASE_SUBMISSION_URL, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
