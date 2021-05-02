import React from 'react';
import { Tab, Icon, Dropdown, TextArea, Button, GridRow } from 'semantic-ui-react';

import { AssignmentCardElement, convertFeedbacksToFeedbackList } from '../CardGroup';

import './TabUI.css';

export const MyTab = (props) => {
    let tabPanes = props.tabPanes;
    tabPanes = tabPanes?.map((pane) => {
        return {
            menuItem: pane.title,
            render: () => (
                <Tab.Pane as="div" attached={false}>
                    {pane.content}
                </Tab.Pane>
            ),
        };
    });
    return <Tab menu={{ secondary: true, pointing: true, color: 'red' }} style={{ width: '75%' }} panes={tabPanes} />;
};

export const FeedbacksPane = (props) => {
    return (
        <div>
            {props.feedbacksAccordion}
            {props.newCommentButton}
        </div>
    );
};

export const GradePane = (props) => {
    return (
        <>
            {props.tables}
            {props.finalGrade ? <div>Final Grade: {props.finalGrade}</div> : null}
        </>
    );
};

export const SubmissionPane = (props) => {
    let submissionCard;

    if (!props.submission) {
        submissionCard = <div>Not submitted yet</div>;
    } else if (props.submission === 'anonim') {
        submissionCard = <div>This assignment is anonim</div>;
    } else {
        submissionCard = (
            <AssignmentCardElement
                title={props.isLate ? 'LATE - Submission' : 'Submission'}
                fileIcon={props.submission.hasFile ? <Icon name="file" /> : null}
                fileClicked={props.onSubmissionFileClicked}
                date={props.submission.date}
                caption={props.assignment.caption}></AssignmentCardElement>
        );
    }

    let statusIcon = null;
    if (props.assignment.status === 'graded') {
        statusIcon = <Icon name="check circle outline" />;
    } else if (props.assignment.status === 'submitted') {
        statusIcon = <Icon name="clock outline" />;
    } else if (props.assignment.status === 'notsubmitted') {
        statusIcon = <Icon name="remove circle" />;
    }

    return (
        <div>
            <AssignmentCardElement
                title={props.assignment.title}
                titleIcon={statusIcon}
                fileIcon={props.assignment.hasFile ? <Icon name="file" color="grey" /> : null}
                fileClicked={props.onAssignmentFileClicked}
                status={props.assignment.status}
                date={props.assignment.date}
                publisher={props.assignment.publisher}
                caption={props.assignment.caption}></AssignmentCardElement>
            <hr />
            {submissionCard}
            {props.buttons}
        </div>
    );
};

export const StudentPeerReviewPane = (props) => {
    const options = [];
    const points = [];

    for (var i = 0; i < props.group.members.length; i++) {
        if (props.curUser.userId !== props.group.members[i].userId) {
            options.push({
                text: props.group.members[i].name,
                key: props.group.members[i].userId,
                value: props.group.members[i].userId,
            });
        }
    }
    for (var i = 0; i <= props.maxGrade; i++) {
        points.push({
            text: i,
            key: i,
            value: i,
        });
    }
    return (
        <>
            <GridRow className="rows">
                <Dropdown
                    placeholder="Select Peer"
                    onChange={(e, d) => props.changePeer(d.value)}
                    selection
                    options={options}></Dropdown>
            </GridRow>
            <GridRow className="rows">
                <TextArea
                    value={props.comment}
                    onChange={(e, d) => props.commentChange(d)}
                    style={{
                        maxWidth: '200px',
                        minWidth: '200px',
                        minHeight: '100px',
                        maxHeight: '300px',
                    }}></TextArea>
            </GridRow>
            <GridRow className="rows">
                Grade
                <Dropdown
                    onChange={(e, d) => props.gradeChange(d)}
                    value={props.grade}
                    style={{ marginLeft: '20px', marginRight: '20px' }}
                    placeholder="#"
                    className="numberDropdown"
                    selection
                    options={points}></Dropdown>
            </GridRow>
            <GridRow className="rows">
                <Button onClick={() => props.submitReview()} icon labelPosition="right" color="green">
                    Submit
                    <Icon name="send" />
                </Button>
            </GridRow>
        </>
    );
};

export const InstructorPeerReviewPane = (props) => {
    const options = [];
    for (let i = 0; i < props.group.members.length; i++) {
        options.push({
            text: props.group.members[i].name,
            key: props.group.members[i].userId,
            value: props.group.members[i].userId,
        });
    }
    const visibleReviews = [];
    for (var i in props.peerReviews) {
        visibleReviews.push({
            name: props.peerReviews[i].reviewerId,
            caption: props.peerReviews[i].comment,
            grade: props.peerReviews[i].grade,
            maxGrade: props.peerReviews[i].maxGrade,
            date: props.peerReviews[i].createdAt,
            userId: props.peerReviews[i].revieweeId,
        });
    }
    console.log(visibleReviews);

    return (
        <>
            <GridRow className="rows">
                <Dropdown
                    placeholder="Select Peer"
                    onChange={(e, d) => props.changePeer(d.value)}
                    selection
                    options={options}></Dropdown>
            </GridRow>
            <GridRow className="rows">
                {convertFeedbacksToFeedbackList(
                    visibleReviews, //
                    () => {},
                    () => {},
                    props.userId,
                    () => {}
                )}
            </GridRow>
        </>
    );
};
