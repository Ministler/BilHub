import { Card, Icon, Button } from 'semantic-ui-react';

import { AssignmentCardElement, FeedbackCardElement } from './CardGroupUI';

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

    return <Card.Group>{assignmentCardElements}</Card.Group>;
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

    return <Card.Group>{feedbackCardElements}</Card.Group>;
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
            <Card.Group>
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
