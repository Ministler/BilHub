import React from 'react';
import { Tab, Card } from 'semantic-ui-react';

import { AssignmentCardElement } from '../CardGroup';

export const MyTab = (props) => {
    let tabPanes = props.tabPanes;
    tabPanes = tabPanes?.map((pane) => {
        return {
            menuItem: pane.title,
            render: () => <Tab.Pane>{pane.content}</Tab.Pane>,
        };
    });
    return <Tab panes={tabPanes} />;
};

export const FeedbacksPane = (props) => {
    return (
        <>
            {props.feedbacksAccordion}
            {props.newCommentButton}
        </>
    );
};

export const GradePane = (props) => {
    return (
        <div>
            {props.tables}
            {props.finalGrade ? <div>Final Grade: {props.finalGrade}</div> : null}
        </div>
    );
};

export const AssignmentPane = (props) => {
    return (
        <Card.Group as="div" className="AssignmentCardGroup">
            {props.feedList}
        </Card.Group>
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
                title={'Submission'}
                file={props.submission.file}
                fileClicked={props.onSubmissionFileClicked}
                date={props.submission.date}>
                {props.submission.caption}
            </AssignmentCardElement>
        );
    }

    return (
        <div>
            <AssignmentCardElement
                title={props.assignment.title}
                file={props.assignment.file}
                fileClicked={props.onAssignmentfileClicked}
                status={props.assignment.status}
                date={props.assignment.date}
                publisher={props.assignment.publisher}>
                {props.assignment.caption}
            </AssignmentCardElement>
            <hr />
            {submissionCard}
            {props.buttons}
        </div>
    );
};
