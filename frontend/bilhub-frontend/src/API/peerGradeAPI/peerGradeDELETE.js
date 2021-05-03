import { authAxios, BASE_PEER_GRADE_URL } from '../axiosConfigs';

export const deletePeerGradeRequest = async (peerGradeId) => {
    const url = peerGradeId;

    return authAxios
        .delete(BASE_PEER_GRADE_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
