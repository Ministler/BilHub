import { Card, Icon, Button } from 'semantic-ui-react';
import { convertMembersToMemberElement } from '../BriefList';

import { AssignmentCardElement, FeedbackCardElement, RequestCardElement } from './CardGroupUI';

export const convertAssignmentsToAssignmentList = (assignments, onAssignmentClicked, onAssignmentFileClicked) => {
    const assignmentCardElements = assignments.map((assignment) => {
        const date = 'Publishment Date: ' + assignment.publishmentDate + ' / Due Date: ' + assignment.dueDate;

        let titleIcon = null;
        if (assignment.status === 'graded') {
            titleIcon = <Icon name="check circle outline" />;
        } else if (assignment.status === 'submitted') {
            titleIcon = <Icon name="clock outline" />;
        } else if (assignment.status === 'notsubmitted') {
            titleIcon = <Icon name="remove circle" />;
        }

        const fileIcon = assignment.file ? <Icon name="file" size="big" /> : null;

        return (
            <AssignmentCardElement
                title={assignment.title}
                titleIcon={titleIcon}
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
    requestStatus,
    onUserClicked,
    onCourseClicked,
    onRequestApproved,
    onRequestDisapproved
) => {
    return (
        <Card.Group as="div" className="AssignmentCardGroup">
            {requests?.map((request) => {
                let yourGroup = null;
                if (request.yourGroup) {
                    yourGroup = convertMembersToMemberElement(request.yourGroup, onUserClicked);
                }

                let otherGroup = null;
                if (request.otherGroup) {
                    yourGroup = convertMembersToMemberElement(request.otherGroup, onUserClicked);
                }

                let titleStart, titleMid, userName, userId;
                let voteIcons = null;

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
                    titleMid = ' wants to ' + request.type + ' Unformed Group in ';

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
                    titleMid = "'s " + request.type + ' request ' + request.status + ' by your Unformed Group in ';
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
            })}
        </Card.Group>
    );
};
