import React from 'react';

import {
    BriefList,
    TitledIconedBriefElement,
    TitledDatedBriefElement,
    MemberBriefElement,
    SubmissionBriefElement,
    GroupBriefElement,
} from './BriefListUI';
import { Icon, Segment, Label, Popup, Header, Grid, Button, Dropdown } from 'semantic-ui-react';
export const convertMyProjectsToBriefList = (myProjects, onMyProjectClicked) => {
    const myProjectBriefElements = myProjects.map((project) => {
        const icon = project.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
        const title = project.courseCode + '/' + project.projectName;
        return (
            <TitledIconedBriefElement icon={icon} title={title} onClick={() => onProjectClicked(project.projectId)} />
        );
    });

    return <BriefList title={'My Projects'}>{myProjectBriefElements}</BriefList>;
};

export const convertInstructedCoursesToBriefList = (instructedCourses, onCourseClicked) => {
    const instructedCourseBriefElements = instructedCourses.map((course) => {
        const icon = course.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
        const title = course.courseCode;
        return <TitledIconedBriefElement icon={icon} title={title} onClick={() => onCourseClicked(course.courseId)} />;
    });

    return <BriefList title={'Instructed Courses'}>{instructedCourseBriefElements}</BriefList>;
};

export const convertUpcomingAssignmentsToBriefList = (upcomingAssignments, onAssignmentClicked) => {
    const upcomingAssignmentsBriefElements = upcomingAssignments ? (
        upcomingAssignments.map((assignment) => {
            const title = assignment.courseCode + '/' + assignment.assignmentName;
            return (
                <TitledDatedBriefElement
                    title={title}
                    date={assignment.dueDate}
                    onClick={() => onAssignmentClicked(assignment.projectId, assignment.submissionId)}
                />
            );
        })
    ) : (
        <div>You Have No Upcoming Assignments</div>
    );

    return <BriefList title={'Upcoming Assignments'}>{upcomingAssignmentsBriefElements}</BriefList>;
};

export const convertNotGradedAssignmentsToBriefList = (notGradedAssignments, onAssignmentClicked) => {
    const notGradedAssignmentsBriefElements = notGradedAssignments.map((assignment) => {
        const title = assignment.courseCode + '/' + assignment.assignmentName;
        return (
            <TitledDatedBriefElement
                title={title}
                date={assignment.dueDate}
                onClick={() => onAssignmentClicked(assignment.courseId, assignment.assignmentId)}
            />
        );
    });

    return <BriefList title={'Not Graded Assignments'}>{notGradedAssignmentsBriefElements}</BriefList>;
};

export const convertMembersToMemberElement = (members, onUserClicked) => {
    return members?.map((member) => {
        return <MemberBriefElement onClick={() => onUserClicked(member.userId)} member={member} />;
    });
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
                onSubmissionFileClicked={() => onSubmissionFileClicked()}
            />
        );
    });
};

export const convertUnformedGroupsToBriefList = (props) => {
    return props.groups?.map((group) => {
        return (
            <Popup
                on="click"
                trigger={
                    <Segment className="clickableHighlightBack" style={{ width: '15%', float: 'left', margin: '10px' }}>
                        <GroupBriefElement group={group.members} />
                    </Segment>
                }
                flowing>
                <Button>Unform Group</Button>
            </Popup>
        );
    });
};

export const convertFormedGroupsToBriefList = (props) => {
    return props.groups?.map((group) => {
        var groups = [];
        for (var i = 0; i < props.groups.length; i++) {
            if (group.name != props.groups[i].name) {
                groups.push({
                    key: props.groups[i].name,
                    text: props.groups[i].name,
                    value: props.groups[i].name,
                });
            }
        }
        var students = [];
        for (var i = 0; i < group.members.length; i++) {
            students.push({
                key: group.members[i],
                text: group.members[i],
                value: group.members[i],
            });
        }
        return (
            <Popup
                trigger={
                    <Segment className="clickableHighlightBack" style={{ width: '15%', float: 'left', margin: '10px' }}>
                        <Label style={{ textAlign: 'center' }} attached="top">
                            {group.name}
                        </Label>
                        <GroupBriefElement group={group.members} />
                    </Segment>
                }
                flowing
                on="click">
                <Grid centered divided columns={3}>
                    <Grid.Column textAlign="center">
                        <Header as="h4">Ungroup This Group</Header>
                        <Button>Ungroup</Button>
                    </Grid.Column>
                    <Grid.Column textAlign="center">
                        <Header as="h4">Remove Student From Group</Header>
                        <Dropdown placeholder="Select Student" fluid selection options={students} />
                        <Button>Remove</Button>
                    </Grid.Column>
                    <Grid.Column textAlign="center">
                        <Header as="h4">Merge With Group</Header>
                        <Dropdown placeholder="Select Group" fluid selection options={groups} />
                        <Button>Merge</Button>
                    </Grid.Column>
                </Grid>
            </Popup>
        );
    });
};
