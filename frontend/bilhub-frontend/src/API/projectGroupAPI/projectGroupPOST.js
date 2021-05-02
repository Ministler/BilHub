import { authAxios, BASE_PROJECT_GROUP_URL } from '../axiosConfigs';

export const postProjectGiveReadyRequest = async (projectGroupId, confirmationState) => {
    const url = 'ConfirmationState';
    const body = {
        projectGroupId: projectGroupId,
        confirmationState: confirmationState,
    };

    return authAxios
        .post(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postLeaveGroupRequest = async (projectGroupId) => {
    const url = 'LeftGroup';
    const body = {
        projectGroupId: projectGroupId,
    };

    return authAxios
        .post(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postForceCancelGroupRequest = async (projectGroupId) => {
    const url = 'ForceCancelProjectGroup';
    const body = {
        projectGroupId: projectGroupId,
    };

    return authAxios
        .post(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postKickStudentFromGroupRequest = async (projectGroupId, userId) => {
    const url = 'KickStudentFromGroup';
    const body = {
        projectGroupId: projectGroupId,
        userId: userId,
    };

    return authAxios
        .post(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postCompleteJoinRequest = async (joinRequestId) => {
    const url = 'CompleteJoinRequest';
    const body = {
        joinRequestId: joinRequestId,
    };

    return authAxios
        .post(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postCompleteMergeRequest = async (mergeRequestId) => {
    const url = 'CompleteMergeRequest';
    const body = {
        mergeRequestId: mergeRequestId,
    };

    return authAxios
        .post(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const postUpdateSRSGradeRequest = async (projectGroupId, newSrsGrade, maxSrsGrade) => {
    const url = 'UpdateSrsGrade';
    const body = {
        projectGroupId: projectGroupId,
        newSrsGrade: newSrsGrade,
        maxSrsGrade: maxSrsGrade,
    };

    return authAxios
        .post(BASE_PROJECT_GROUP_URL + url, body)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
