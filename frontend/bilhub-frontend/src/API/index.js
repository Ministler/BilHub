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
export { postCommentRequest } from './commentAPI';
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
