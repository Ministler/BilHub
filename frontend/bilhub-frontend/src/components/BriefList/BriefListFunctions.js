import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Icon, Segment, Label, Popup, Header, Grid, Button, Dropdown, DimmerDimmable } from 'semantic-ui-react';
import { dateObjectToString, convertDate } from '../../utils';

import { SubmissionBriefElement, GroupBriefElement } from './BriefListUI';
import './BriefListUI.css';

export const convertMyProjectsToBriefList = (myProjects, onProjectClicked, onCourseClicked) => {
    const myProjectBriefElements = myProjects.map((project) => {
        const icon = project.isActive ? (
            <Icon name="lock open" style={{ color: 'rgb(196, 126, 5)' }} />
        ) : (
            <Icon name="lock" style={{ color: 'rgb(196, 126, 5)' }} />
        );
        const title = project.isLocked
            ? project.projectName !== null
                ? project.courseCode + '/' + project.projectName
                : project.courseCode + '/' + 'Formed Group'
            : project.courseCode + '/' + 'Unformed Group';

        const onClick = project.isLocked
            ? () => onProjectClicked(project.projectId)
            : () => onCourseClicked(project.courseId);

        return (
            <p className={'BriefListElements'}>
                <Link style={{ fontWeight: 'bold' }} onClick={onClick}>
                    {icon}
                    {title}
                </Link>
            </p>
        );
    });

    return <div>{myProjectBriefElements}</div>;
};

export const convertInstructedCoursesToBriefList = (instructedCourses, onCourseClicked) => {
    const instructedCourseBriefElements = instructedCourses.map((course) => {
        const icon = course.isActive ? (
            <Icon name="lock open" style={{ color: 'rgb(196, 126, 5)' }} />
        ) : (
            <Icon name="lock" style={{ color: 'rgb(196, 126, 5)' }} />
        );
        const title = course.courseCode;
        return (
            <p className={'BriefListElements'}>
                <Link style={{ fontWeight: 'bold' }} onClick={() => onCourseClicked(course.courseId)}>
                    {icon}
                    {title}
                </Link>
            </p>
        );
    });

    return <div>{instructedCourseBriefElements}</div>;
};

export const convertUpcomingAssignmentsToBriefList = (upcomingAssignments, onAssignmentClicked) => {
    const upcomingAssignmentsBriefElements =
        upcomingAssignments && upcomingAssignments.length !== 0 ? (
            upcomingAssignments.map((assignment) => {
                const title = assignment.courseCode + '/' + assignment.assignmentName;
                return (
                    <Segment>
                        <Link onClick={() => onAssignmentClicked(assignment.projectId, assignment.submissionId)}>
                            {title}
                        </Link>
                        <div align="right" className="DueDate">
                            {typeof assignment.dueDate === 'object'
                                ? convertDate(dateObjectToString(assignment.dueDate))
                                : convertDate(assignment.dueDate)}
                        </div>
                    </Segment>
                );
            })
        ) : (
            <div></div>
        );

    return upcomingAssignments && upcomingAssignments.length !== 0 ? (
        <Segment.Group raised>
            <Label attached="top" style={{ backgroundColor: 'rgb(33,133,208)', color: 'white', textAlign: 'center' }}>
                Upcoming Assignments
            </Label>
            {upcomingAssignmentsBriefElements}
        </Segment.Group>
    ) : (
        <div></div>
    );
};

export const convertNotGradedAssignmentsToBriefList = (notGradedAssignments, onAssignmentClicked) => {
    const notGradedAssignmentsBriefElements =
        notGradedAssignments && notGradedAssignments.length !== 0 ? (
            notGradedAssignments.map((assignment) => {
                const title = assignment.courseCode + '/' + assignment.assignmentName;
                return (
                    <Segment>
                        <Link onClick={() => onAssignmentClicked(assignment.courseId, assignment.assignmentId)}>
                            {title}
                        </Link>
                        <div align="right" className="DueDate">
                            {typeof assignment.dueDate === 'object'
                                ? convertDate(dateObjectToString(assignment.dueDate))
                                : convertDate(assignment.dueDate)}
                        </div>
                    </Segment>
                );
            })
        ) : (
            <div></div>
        );

    return notGradedAssignments && notGradedAssignments.length !== 0 ? (
        <Segment.Group raised>
            <Label attached="top" style={{ backgroundColor: 'rgb(219,40,40)', color: 'white', textAlign: 'center' }}>
                Not Graded Assignments
            </Label>
            {notGradedAssignmentsBriefElements}
        </Segment.Group>
    ) : (
        <div></div>
    );
};

export const convertMembersToMemberElement = (members, onUserClicked, title = '') => {
    const convertedList = members?.map((member) => {
        return (
            <p>
                <Link
                    style={{ fontWeight: 'bold' }}
                    onClick={() => onUserClicked(member.userId ? member.userId : member.id)}>
                    {member.name}
                </Link>
            </p>
        );
    });
    return (
        <div>
            <h4 style={{ marginLeft: '20px' }}>{title}</h4>
            {convertedList}
        </div>
    );
};

export const convertSubmissionsToSubmissionElement = (
    submissions,
    onSubmissionPageClicked,
    onSubmissionFileClicked
) => {
    return submissions?.map((submission) => {
        return (
            <SubmissionBriefElement
                submission={submission}
                onSubmissionPageClicked={() => onSubmissionPageClicked(submission.projectId, submission.submissionId)}
                onSubmissionFileClicked={() => onSubmissionFileClicked(submission.submissionId, submission.fileName)}
            />
        );
    });
};

export const convertUnformedGroupsToBriefList = (groups, ungroup) => {
    return groups?.map((group) => {
        return (
            <Popup
                on="click"
                trigger={
                    <Segment className="clickableHighlightBack" style={{ width: '15%', float: 'left', margin: '10px' }}>
                        <GroupBriefElement group={group.members} />
                    </Segment>
                }
                flowing>
                <Button onClick={() => ungroup(group.groupId)}>Unform Group</Button>
            </Popup>
        );
    });
};

export const FormedGroupsBriefList = (props) => {
    const [currentStudentId, setCurrentStudentId] = useState(-1);
    const [currentGroupId, setCurrentGroupId] = useState(-1);

    return props.groups?.map((group) => {
        let otherGroups = [];
        for (let i = 0; i < props.groups.length; i++) {
            if (group.groupName !== props.groups[i].groupName) {
                otherGroups.push({
                    key: props.groups[i].groupId,
                    text: props.groups[i].groupName,
                    value: props.groups[i].groupId,
                });
            }
        }
        let students = [];
        for (let i = 0; i < group.members.length; i++) {
            students.push({
                key: group.members[i].userId,
                text: group.members[i].name,
                value: group.members[i].userId,
            });
        }
        return (
            <Popup
                trigger={
                    <Segment className="clickableHighlightBack" style={{ width: '15%', float: 'left', margin: '10px' }}>
                        <Label style={{ textAlign: 'center' }} attached="top">
                            {group.groupName}
                        </Label>
                        <GroupBriefElement group={group.members} />
                    </Segment>
                }
                flowing
                on="click">
                <Grid centered divided columns={3}>
                    <Grid.Column textAlign="center">
                        <Header as="h4">Ungroup This Group</Header>
                        <Button onClick={() => props.ungroup(group.groupId)}>Ungroup</Button>
                    </Grid.Column>
                    <Grid.Column textAlign="center">
                        <Header as="h4">Remove Student From Group</Header>
                        <Dropdown
                            placeholder="Select Student"
                            fluid
                            selection
                            options={students}
                            onChange={(e, data) => setCurrentStudentId(data.value)}
                        />
                        <Button onClick={() => props.removeStudent(group.groupId, currentStudentId)}>Remove</Button>
                    </Grid.Column>
                    <Grid.Column textAlign="center">
                        <Header as="h4">Merge With Group</Header>
                        <Dropdown
                            placeholder="Select Group"
                            fluid
                            selection
                            options={otherGroups}
                            onChange={(e, data) => setCurrentGroupId(data.value)}
                        />
                        <Button onClick={() => props.mergeGroup(group.groupId, currentGroupId)}>Merge</Button>
                    </Grid.Column>
                </Grid>
            </Popup>
        );
    });
};
