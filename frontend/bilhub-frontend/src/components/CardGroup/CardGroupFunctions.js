import { Card, Icon, Button } from 'semantic-ui-react';
import { dateObjectToString } from '../../utils';
import { convertMembersToMemberElement } from '../BriefList';

import { AssignmentCardElement, FeedbackCardElement, RequestCardElement } from './CardGroupUI';

export const convertAssignmentsToAssignmentList = (
    assignments,
    onAssignmentClicked,
    onSubmissionClicked,
    onAssignmentFileClicked,
    assignmentIcons
) => {
    const assignmentCardElements = assignments?.map((assignment) => {
        const date =
            'Publishment Date: ' +
            (typeof assignment.publishmentDate === 'object'
                ? dateObjectToString(assignment.publishmentDate)
                : assignment.publishmentDate) +
            ' / Due Date: ' +
            (typeof assignment.dueDate === 'object' ? dateObjectToString(assignment.dueDate) : assignment.dueDate);

        let statusIcon = null;
        if (assignment.status === 'graded') {
            statusIcon = <Icon name="check circle outline" style={{ marginLeft: '5px' }} color="blue" />;
        } else if (assignment.status === 'submitted') {
            statusIcon = <Icon name="clock outline" style={{ marginLeft: '5px', color: 'rgb(251, 178, 4)' }} />;
        } else if (assignment.status === 'notsubmitted') {
            statusIcon = <Icon name="remove circle" style={{ marginLeft: '5px' }} color="red" />;
        }

        const fileIcon = assignment.hasFile ? <Icon name="file" color="grey" /> : null;

        let onAssignmentClickedId = assignment.submissionId
            ? () => onSubmissionClicked(assignment.projectId, assignment.submissionId)
            : () => onAssignmentClicked(assignment.courseId, assignment.assignmentId);

        return (
            <AssignmentCardElement
                title={assignment.title}
                titleIcon={statusIcon || assignmentIcons}
                titleClicked={onAssignmentClickedId}
                caption={assignment.caption}
                fileIcon={fileIcon}
                fileClicked={() =>
                    onAssignmentFileClicked(assignment.submissionId ? assignment.submissionId : assignment.assignmentId)
                }
                date={date}
                publisher={assignment.publisher}
            />
        );
    });

    if (!assignmentCardElements) {
        return <div>You Dont Have Any New Feed</div>;
    }

    return (
        <Card.Group as="div" className="AssignmentCardGroup">
            {assignmentCardElements}
        </Card.Group>
    );
};

export const convertNewFeedbacksToFeedbackList = (newFeedbacks, onSubmissionClicked, onProjectClicked) => {
    const newFeedbackCardElements = newFeedbacks ? (
        newFeedbacks.map((feedback) => {
            let titleElement;
            if (feedback.submission) {
                titleElement = (
                    <>
                        <span
                            onClick={() =>
                                onSubmissionClicked(feedback.submission?.projectId, feedback.submission?.submissionId)
                            }>
                            {' '}
                            {feedback.course?.courseName} / {feedback.user?.name} /{' '}
                            {feedback.submission?.assignmentName}{' '}
                        </span>
                    </>
                );
            } else if (feedback.project) {
                titleElement = (
                    <>
                        <span onClick={() => onProjectClicked(feedback.project?.projectId)}>
                            {' '}
                            {feedback.course?.courseName} / {feedback.user?.name} / {feedback.project?.projectName}{' '}
                        </span>
                    </>
                );
            }
            return (
                <FeedbackCardElement 
                    titleElement={titleElement}
                    caption={feedback.feedback?.caption}
                    grade={feedback.feedback?.grade}
                    date={feedback.feedback?.date}
                />
            );
        })
    ) : (
        <div>No Comments Yet</div>
    );

    return newFeedbackCardElements;
};

export const convertFeedbacksToFeedbackList = (
    feedbacks,
    onOpenModal,
    onAuthorClicked,
    userId,
    onFeedbackFileClicked
) => {
    const feedbackCardElements = feedbacks ? (
        feedbacks.map((feedback) => {
            let icons = null;
            if (userId === feedback.userId) {
                icons = (
                    <span>
                        <Icon
                            name="edit"
                            onClick={() =>
                                onOpenModal(
                                    'isEditFeedbackOpen',
                                    false,
                                    feedback.commentId,
                                    feedback.caption,
                                    feedback.grade
                                )
                            }
                        />
                        <Icon
                            name="delete"
                            onClick={() =>
                                onOpenModal(
                                    'isDeleteFeedbackOpen',
                                    false,
                                    feedback.commentId,
                                    feedback.caption,
                                    feedback.grade
                                )
                            }
                        />
                    </span>
                );
            }

            return (
                <FeedbackCardElement
                    author={feedback.name ? feedback.name : 'Comment is anonymous'}
                    caption={feedback.caption}
                    hasFile={feedback.hasFile}
                    onFeedbackFileClicked={() => onFeedbackFileClicked(feedback.commentId)}
                    grade={feedback.grade ? feedback.grade : 'Grade is anonymous'}
                    maxGrade={feedback?.maxGrade}
                    date={feedback.date}
                    icons={icons}
                    onAuthorClicked={() => onAuthorClicked(feedback.userId)}
                />
            );
        })
    ) : (
        <div>No Comments Yet</div>
    );

    return (
        <Card.Group as="div" className="AssignmentCardGroup">
            {feedbackCardElements}
        </Card.Group>
    );
};

export const convertSRSFeedbackToSRSCardElement = (
    SRSResult,
    isTAorInstructor,
    onModalOpenedWithComments,
    onAuthorClicked,
    onModalOpened
) => {
    if (SRSResult) {
        let icons = null;
        if (isTAorInstructor) {
            icons = (
                <span>
                    <Icon
                        name="delete"
                        color="red"
                        style={{ float: 'right' }}
                        onClick={() =>
                            onModalOpenedWithComments(
                                'isDeleteFeedbackOpen',
                                true,
                                SRSResult.commentId,
                                SRSResult.caption,
                                SRSResult.grade,
                                SRSResult.hasFile,
                                SRSResult.maxGrade
                            )
                        }
                    />
                    <Icon
                        name="edit"
                        color="blue"
                        style={{ float: 'right' }}
                        onClick={() =>
                            onModalOpenedWithComments(
                                'isEditFeedbackOpen',
                                true,
                                SRSResult.commentId,
                                SRSResult.caption,
                                SRSResult.grade,
                                SRSResult.hasFile,
                                SRSResult.maxGrade
                            )
                        }
                    />
                </span>
            );
        }

        return (
            <Card.Group as="div" className="AssignmentCardGroup">
                <FeedbackCardElement
                    author={SRSResult.name}
                    onAuthorClicked={() => onAuthorClicked(SRSResult.userId)}
                    caption={SRSResult.caption}
                    grade={SRSResult.grade}
                    date={SRSResult.date}
                    icons={icons}
                    maxGrade={SRSResult.maxGrade}
                    isSrs={true}
                />
            </Card.Group>
        );
    } else if (isTAorInstructor) {
        return <Button onClick={() => onModalOpened('isGiveFeedbackOpen', true)}>Add SRS Grade</Button>;
    } else {
        return <div>No SRS Feedback</div>;
    }
};

export const convertRequestsToRequestsList = (
    requests,
    requestsType,
    requestStatus,
    onUserClicked,
    onRequestAction
) => {
    return (
        <Card.Group as="div" className="AssignmentCardGroup">
            {requests ? (
                requests.map((request) => {
                    let yourGroup = null;
                    if (request.yourGroup) {
                        yourGroup = convertMembersToMemberElement(request.yourGroup, onUserClicked);
                    }

                    let otherGroup = null;
                    if (request.otherGroup) {
                        otherGroup = convertMembersToMemberElement(request.otherGroup, onUserClicked);
                    }

                    let titleStart, titleMid, userName, userId;
                    let voteIcons = null;

                    if (requestsType === 'incoming') {
                        if (request.type === 'Join') {
                            userName = request.user?.name;
                            userId = request.user?.userId;
                        }

                        if (request.type === 'Merge') {
                            const requestOwner = request.otherGroup?.find((user) => {
                                return user.requestOwner;
                            });

                            userName = requestOwner?.name;
                            userId = requestOwner?.userId;
                        }

                        if (requestStatus === 'pending') {
                            titleMid = ' wants to ' + request.type + ' your group ';

                            voteIcons = (
                                <>
                                    <p style={{ display: 'inline' }}>Approved: {request.voteStatus}&nbsp;</p>
                                    <Icon
                                        onClick={() =>
                                            onRequestAction(
                                                'isApprovalModalOpen',
                                                request.requestId,
                                                request.type,
                                                userName
                                            )
                                        }
                                        name="checkmark"
                                        color="blue"
                                    />
                                    <Icon
                                        onClick={() =>
                                            onRequestAction(
                                                'isDisapprovalModalOpen',
                                                request.requestId,
                                                request.type,
                                                userName
                                            )
                                        }
                                        name="x"
                                        color="red"
                                    />
                                </>
                            );
                        }

                        if (requestStatus === 'unresolved') {
                            titleStart = 'You approved ' + request.type + ' request of ';
                            voteIcons = (
                                <p style={{ display: 'inline' }}>
                                    Approved: {request.voteStatus}&nbsp;
                                    {
                                        <Icon
                                            onClick={() =>
                                                onRequestAction(
                                                    'isUndoModalOpen',
                                                    request.requestId,
                                                    request.type,
                                                    userName
                                                )
                                            }
                                            name="undo"
                                            color="purple"
                                        />
                                    }
                                </p>
                            );
                        }

                        if (requestStatus === 'resolved') {
                            titleStart = request.type + ' request of ';
                            titleMid = ' ' + request.status;
                        }
                    } else if (requestsType === 'outgoing') {
                        if (request.type === 'Join') {
                            userName = 'Your ';
                        }

                        if (request.type === 'Merge') {
                            if (request.isRequestOwner) {
                                userName = 'You ';
                            } else {
                                const requestOwner = request.yourGroup?.find((user) => {
                                    return user.requestOwner;
                                });
                                userName = requestOwner?.name;
                                userId = requestOwner?.userId;
                            }
                        }

                        if (requestStatus === 'pending') {
                            titleMid = ' send a ' + request.type + ' request';

                            voteIcons = (
                                <>
                                    <p style={{ display: 'inline' }}>Approved: {request.voteStatus}&nbsp;</p>
                                    <Icon
                                        onClick={() =>
                                            onRequestAction(
                                                'isApprovalModalOpen',
                                                request.requestId,
                                                request.type,
                                                userName
                                            )
                                        }
                                        name="checkmark"
                                        color="blue"
                                    />
                                    <Icon
                                        onClick={() =>
                                            onRequestAction(
                                                'isDisapprovalModalOpen',
                                                request.requestId,
                                                request.type,
                                                userName
                                            )
                                        }
                                        name="x"
                                        color="red"
                                    />
                                </>
                            );
                        }

                        if (requestStatus === 'unresolved') {
                            titleMid = ' send a ' + request.type + ' request';

                            voteIcons = (
                                <>
                                    <p style={{ display: 'inline' }}>
                                        Approved: {request.voteStatus}&nbsp;{' '}
                                        {request.type === 'Merge' ? (
                                            <Icon
                                                onClick={() =>
                                                    onRequestAction(
                                                        'isUndoModalOpen',
                                                        request.requestId,
                                                        request.type,
                                                        userName
                                                    )
                                                }
                                                name="undo"
                                                color="purple"
                                            />
                                        ) : (
                                            <Icon
                                                onClick={() =>
                                                    onRequestAction(
                                                        'isDeleteModalOpen',
                                                        request.requestId,
                                                        request.type,
                                                        userName
                                                    )
                                                }
                                                name="x"
                                                color="red"
                                            />
                                        )}
                                    </p>
                                </>
                            );
                        }

                        if (requestStatus === 'resolved') {
                            titleMid = ' ' + request.type + ' request ' + request.status;
                        }
                    }

                    return (
                        <RequestCardElement
                            titleStart={titleStart}
                            titleMid={titleMid}
                            userName={userName}
                            message={request.message}
                            courseName={request.course}
                            yourGroup={yourGroup}
                            otherGroup={otherGroup}
                            voteStatus={request.voteStatus}
                            voteIcons={voteIcons}
                            requestDate={request.requestDate}
                            formationDate={request.formationDate}
                            approvalDate={request.approvalDate}
                            disapprovalDate={request.disapprovalDate}
                            onUserClicked={() => onUserClicked(userId)}
                        />
                    );
                })
            ) : (
                <div>
                    You Dont have any {requestsType} {requestStatus} requests
                </div>
            )}
        </Card.Group>
    );
};
