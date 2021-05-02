import { authAxios, BASE_PROJECT_GROUP_URL } from '../axiosConfigs';

export const getProjectGroupRequest = async (id) => {
    const url = id;

    return authAxios
        .get(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getSectionGroupsRequest = async (sectionId) => {
    const url = 'ProjectGroupsOfSection?sectionId=' + sectionId;

    return authAxios
        .get(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getUserGroupsRequest = async (userId) => {
    const url = 'ProjectGroupsOfUser?userId=' + userId;

    return authAxios
        .get(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getGroupInstructorCommentsRequest = async (groupId) => {
    const url = 'InstructorComments/' + groupId;

    return authAxios
        .get(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getGroupTACommentsRequest = async (groupId) => {
    const url = 'TAComments/' + groupId;

    return authAxios
        .get(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getGroupStudentCommentsRequest = async (groupId) => {
    const url = 'StudentComments/' + groupId;

    return authAxios
        .get(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};

export const getGroupAssignmentsRequest = async (ProjectGroupId) => {
    const url = 'Assignments/' + ProjectGroupId;

    return authAxios
        .get(BASE_PROJECT_GROUP_URL + url)
        .then((response) => response)
        .catch((error) => {
            throw error;
        });
};
