export {
    loginRequest,
    checkAuthRequest,
    changePassword,
    registerRequest,
    resendRequest,
    verifyRequest,
    forgotPasswordRequest,
    getIdByEmailRequest,
} from './authAPI';
export {} from './userAPI';
export {} from './commentAPI';
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
export { getUserGroupsRequest } from './projectGroupAPI';
export {
    getIncomingJoinRequest,
    getIncomingMergeRequest,
    getOutgoingMergeRequest,
    getOutgoingJoinRequest,
} from './requestAPI';
export { getNewCommentsRequest, getSubmissionRequest } from './submissionAPI';
