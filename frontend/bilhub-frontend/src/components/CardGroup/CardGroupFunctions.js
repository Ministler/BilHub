import { Card, Icon, Button } from 'semantic-ui-react';
import { convertMembersToMemberElement } from '../BriefList';

import { AssignmentCardElement, FeedbackCardElement, RequestCardElement } from './CardGroupUI';

export const convertAssignmentsToAssignmentList = (
    assignments,
    onAssignmentClicked,
    onAssignmentFileClicked,
    assignmentIcons
) => {
    const assignmentCardElements = assignments.map((assignment) => {
        const date = 'Publishment Date: ' + assignment.publishmentDate + ' / Due Date: ' + assignment.dueDate;

        let statusIcon = null;
        if (assignment.status === 'graded') {
            statusIcon = <Icon name="check circle outline" />;
        } else if (assignment.status === 'submitted') {
            statusIcon = <Icon name="clock outline" />;
        } else if (assignment.status === 'notsubmitted') {
            statusIcon = <Icon name="remove circle" />;
        }

        const fileIcon = assignment.file ? <Icon name="file" size="big" /> : null;

        return (
            <AssignmentCardElement
                title={assignment.title}
                titleIcon={statusIcon || assignmentIcons}
                titleClicked={() => onAssignmentClicked(assignment.submissionPageId)}
                caption={assignment.caption}
                fileIcon={fileIcon}
                fileClicked={onAssignmentFileClicked}
                date={date}
                publisher={assignment.publisher}
            />
        );
    });

    return (
        <Card.Group as="div" className="AssignmentCardGroup">
            {assignmentCardElements}
        </Card.Group>
    );
};

export const convertNewFeedbacksToFeedbackList = (
    newFeedbacks,
    onUserClicked,
    onSubmissionClicked,
    onProjectClicked,
    onCourseClicked
) => {
    const newFeedbackCardElements = newFeedbacks ? (
        newFeedbacks.map((feedback) => {
            let titleElement;
            if (feedback.submission) {
                titleElement = (
                    <>
                        <span onClick={() => onUserClicked(feedback.user?.userId)}> {feedback.user?.name} </span>
                        Commented to Your
                        <span
                            onClick={() =>
                                onSubmissionClicked(
                                    feedback.submission?.projectId,
                                    feedback.submission?.submissionPageId
                                )
                            }>
                            {' '}
                            {feedback.submission?.assignmentName}{' '}
                        </span>
                        Submission in
                        <span onClick={() => onCourseClicked(feedback.course?.courseId)}>
                            {' '}
                            {feedback.course?.courseName}{' '}
                        </span>
                    </>
                );
            } else if (feedback.project) {
                titleElement = (
                    <>
                        <span onClick={() => onUserClicked(feedback.user?.userId)}> {feedback.user?.name} </span>
                        Commented to Your
                        <span onClick={() => onProjectClicked(feedback.project?.projectId)}>
                            {' '}
                            {feedback.project?.projectName}{' '}
                        </span>
                        Project in
                        <span onClick={() => onCourseClicked(feedback.course?.courseId)}>
                            {' '}
                            {feedback.course?.courseName}{' '}
                        </span>
                    </>
                );
            }
            return (
                <FeedbackCardElement
                    titleElement={titleElement}
                    caption={feedback.feedback?.caption}
                    grade={feedback.feedback?.grade}
                    date={feedback.date}
                    onAuthorClicked={() => onUserClicked(feedback.user?.userId)}
                />
            );
        })
    ) : (
        <div>No Comments Yet</div>
    );

    return newFeedbackCardElements;
};

export const convertFeedbacksToFeedbackList = (feedbacks, onOpenModel, onAuthorClicked, userId) => {
    const feedbackCardElements = feedbacks ? (
        feedbacks.map((feedback) => {
            let icons = null;
            if (userId === feedback.userId) {
                icons = (
                    <span>
                        <Icon
                            name="edit"
                            onClick={() =>
                                onOpenModel(
                                    'isEditFeedbackOpen',
                                    false,
                                    feedback.commentId,
                                    feedback.feedback,
                                    feedback.grade
                                )
                            }
                        />
                        <Icon
                            name="delete"
                            onClick={() =>
                                onOpenModel(
                                    'isDeleteFeedbackOpen',
                                    false,
                                    feedback.commentId,
                                    feedback.feedback,
                                    feedback.grade
                                )
                            }
                        />
                    </span>
                );
            }

            return (
                <FeedbackCardElement
                    author={feedback.name}
                    caption={feedback.caption}
                    grade={feedback.grade}
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

export const convertSRSFeedbackToSRSCardElement = (SRSResult, isTAorInstructor, onOpenModal, onAuthorClicked) => {
    if (SRSResult) {
        let icons = null;
        if (isTAorInstructor) {
            icons = (
                <span>
                    <Icon
                        name="edit"
                        onClick={() =>
                            onOpenModal(
                                'isEditFeedbackOpen',
                                true,
                                SRSResult.commentId,
                                SRSResult.feedback,
                                SRSResult.grade
                            )
                        }
                    />
                    <Icon
                        name="delete"
                        onClick={() =>
                            onOpenModal(
                                'isDeleteFeedbackOpen',
                                true,
                                SRSResult.commentId,
                                SRSResult.feedback,
                                SRSResult.grade
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
                />
            </Card.Group>
        );
    } else if (isTAorInstructor) {
        return <Button onClick={() => this.openModal('isGiveFeedbackOpen', true)}>Add SRS Grade</Button>;
    } else {
        return <div>No SRS Feedback</div>;
    }
};

export const convertRequestsToRequestsList = (
    requests,
    requestsType,
    requestStatus,
    onUserClicked,
    onCourseClicked,
    onRequestApproved,
    onRequestDisapproved
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
                            userId = request.user?.Id;
                        }

                        if (request.type === 'Merge') {
                            const requestOwner = request.otherGroup?.find((user) => {
                                return user.requestOwner;
                            });

                            userName = requestOwner?.name;
                            userId = requestOwner?.Id;
                        }

                        if (requestStatus === 'pending') {
                            titleMid = ' created a ' + request.type + ' request to Your Unformed Group in ';

                            voteIcons = (
                                <>
                                    <Icon onClick={onRequestApproved} name="checkmark" />
                                    <Icon onClick={onRequestDisapproved} name="x" />
                                </>
                            );
                        }

                        if (requestStatus === 'unresolved') {
                            titleStart = 'Your Approved ';
                            titleMid = request.type + ' request to your Unformed Group in ';

                            voteIcons = (
                                <>
                                    <Icon name="checkmark" />
                                </>
                            );
                        }

                        if (requestStatus === 'resolved') {
                            titleMid =
                                "'s " + request.type + ' request ' + request.status + ' by your Unformed Group in ';
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
                                console.log();
                                userName = requestOwner?.name;
                                userId = requestOwner?.Id;
                            }
                        }

                        if (requestStatus === 'pending') {
                            titleMid = ' created a ' + request.type + ' request to an Unformed Group in ';

                            voteIcons = (
                                <>
                                    <Icon onClick={onRequestApproved} name="checkmark" />
                                    <Icon onClick={onRequestDisapproved} name="x" />
                                </>
                            );
                        }

                        if (requestStatus === 'unresolved') {
                            titleMid = ' created a ' + request.type + ' request to an Unformed Group in ';

                            voteIcons = (
                                <>
                                    <Icon name="checkmark" />
                                </>
                            );
                        }

                        if (requestStatus === 'resolved') {
                            titleMid =
                                ' ' + request.type + ' request ' + request.status + ' by your Unformed Group in ';
                        }
                    }

                    return (
                        <RequestCardElement
                            titleStart={titleStart}
                            titleMid={titleMid}
                            userName={userName}
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
                            onCourseClicked={() => onCourseClicked(request.courseId)}
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