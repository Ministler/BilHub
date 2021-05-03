import React from 'react';
import { Tab, Icon, Dropdown, TextArea, Button, Form, Divider, GridRow, Grid } from 'semantic-ui-react';

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
        statusIcon = <Icon name="check circle outline" color="blue"/>;
    } else if (props.assignment.status === 'submitted') {
        statusIcon = <Icon name="clock outline" color="rgb(251, 178, 4)"/>;
    } else if (props.assignment.status === 'notsubmitted') {
        statusIcon = <Icon name="remove circle" color="red"/>;
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
            <div class="sixteen wide column">
                <Form reply style={{ width: '95%' }}>
                    <Form.Select width={5}
                        placeholder="Select Peer"
                        onChange={(e, d) => props.changePeer(d.value)}
                        options={options}></Form.Select>
                    <Form.TextArea rows="5" value={props.comment} onChange={(e, d) => props.commentChange(d)} />
                    <Grid><Grid.Row columns={2}><Grid.Column>
                    <Form.Group inline>
                        <label>Grade</label>
                        <Form.Select 
                        floated="left"
                        onChange={(e, d) => props.gradeChange(d)}
                        value={props.grade}
                        placeholder="#"
                        className="numberDropdown"
                        selection
                        options={points}
                        ></Form.Select>
                    </Form.Group>
                    </Grid.Column>
                    <Grid.Column>
                    <Form.Button
                        floated="right"
                        labelPosition="right"
                        icon
                        onClick={() => props.submitReview()}
                        color="green"
                        content="Give Feedback"
                        Compact>
                        Submit
                        <Icon name="send" />
                    </Form.Button>
                    </Grid.Column>
                    </Grid.Row></Grid>
                </Form>
            </div>
        </>
    );
};

export const AllStudentPeerReviewPane = (props) => {
    const sections = [];
    const groups = [];
    const students = [];
    for (var i = 0; i < props.state.currentPeerReviewSections.length; i++) {
        sections.push({
            text: props.state.currentPeerReviewSections[i].sectionId,
            value: props.state.currentPeerReviewSections[i].id,
        });
    }
    for (var i = 0; i < props.state.currentPeerReviewGroups.length; i++) {
        groups.push({
            text: props.state.currentPeerReviewGroups[i].groupName,
            value: props.state.currentPeerReviewGroups[i].groupId,
        });
    }
    for (var i = 0; i < props.state.currentPeerReviewStudents.length; i++) {
        students.push({
            text: props.state.currentPeerReviewStudents[i].name,
            value: props.state.currentPeerReviewStudents[i].userId,
        });
    }

    const visibleReviews = [];
    for (var i in props.state.currentReviews) {
        visibleReviews.push({
            name: props.state.currentReviews[i].reviewerId,
            caption: props.state.currentReviews[i].comment,
            grade: props.state.currentReviews[i].grade,
            maxGrade: props.state.currentReviews[i].maxGrade,
            date: props.state.currentReviews[i].createdAt,
            userId: props.state.currentReviews[i].revieweeId,
        });
    }

    return (
        <>
            {' '}
            <Grid>
                <Grid.Row columns={3}>
                    <Grid.Column>
                        <Dropdown
                            name="currentPeerReviewSection"
                            options={sections}
                            selection
                            placeholder="Select Section"
                            value={props.state.currentPeerReviewSection?.id}
                            onChange={(e, d) => props.handleSectionChange(d)}></Dropdown>
                    </Grid.Column>
                    <Grid.Column>
                        <Dropdown
                            name="currentPeerReviewGroup"
                            options={groups}
                            selection
                            placeholder="Select Group"
                            value={props.state.currentPeerReviewGroup?.groupId}
                            onChange={(e, d) => props.handleGroupChange(d)}></Dropdown>
                    </Grid.Column>
                    <Grid.Column>
                        <Dropdown
                            name="currentPeerReviewStudent"
                            options={students}
                            selection
                            placeHolder="Select Student"
                            value={props.state.currentPeerReviewStudent?.userId}
                            onChange={(e, d) => props.handleStudentChange(d)}></Dropdown>
                    </Grid.Column>
                </Grid.Row>
                <Grid.Row>
                    {convertFeedbacksToFeedbackList(
                        visibleReviews,
                        () => {},
                        () => {},
                        props.userId,
                        () => {}
                    )}
                </Grid.Row>
            </Grid>
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
