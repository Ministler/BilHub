export {
    loginRequest,
    checkAuthRequest,
    changePassword,
    registerRequest,
    resendRequest,
    verifyRequest,
    forgotPasswordRequest,
    getIdByEmailRequest,
    updateProfile,
    getProfileInfo,
} from './authAPI';
export {} from './userAPI';
export { getProjectGradeDownloadByIdRequest } from './projectGradeAPI';
export { postCommentRequest, getCommentFileRequest } from './commentAPI';
export {
    getSubmissionFileRequest,
    deleteSubmissionSrsGradeRequest,
    getSubmissionSrsGradeRequest,
} from './submissionAPI';

export {
    getAssignmentFeedsRequest,
    getUpcomingAssignmentFeedsRequest,
    getNotGradedAssignmentRequest,
    getAssignmentFileRequest,
    getAssignmentRequest,
    deleteAssignmentFileRequest,
    deleteAssignmentRequest,
    putAssignmentRequest,
    postAssignmentFileRequest,
    postAssignmentRequest,
    getAssignmentStatisticsRequest,
} from './assignmentAPI';
export {
    getCourseRequest,
    getCourseStatisticRequest,
    postCourseRequest,
    postCourseInstructorRequest,
    deleteInstructorFromCourseRequest,
    deleteCourseRequest,
    putCourseRequest,
    getInstructedCoursesRequest,
    postDeactivateCourseRequest,
    getCourseAssignmentRequest,
    postActivateCourseRequest,
    postLockCourseRequest,
} from './courseAPI';
export { getSectionRequest, deleteStudentFromSectionRequest, postStudentToSectionRequest } from './sectionAPI';
export {
    getUserGroupsRequest,
    getGroupSrsGradeRequest,
    postLeaveGroupRequest,
    postProjectGiveReadyRequest,
    putProjectGroupRequest,
} from './projectGroupAPI';
export {
    getIncomingJoinRequest,
    getIncomingMergeRequest,
    getOutgoingMergeRequest,
    getOutgoingJoinRequest,
    postMergeRequest,
    postJoinRequest,
    putMergeRequest,
    putJoinRequest,
    deleteJoinRequest,
} from './requestAPI';
export {
    deleteSubmissionRequest,
    deleteSubmissionFileRequest,
    getNewCommentsRequest,
    getSubmissionRequest,
    postSubmissionRequest,
    postSubmissionFileRequest,
    putSubmissionRequest,
    postSubmissionSrsGradeRequest,
} from './submissionAPI';
