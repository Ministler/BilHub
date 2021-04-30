import React from 'react';
import { Link } from 'react-router-dom';
import {
    BriefList,
    TitledIconedBriefElement,
    TitledDatedBriefElement,
    MemberBriefElement,
    SubmissionBriefElement,
    GroupBriefElement,
} from './BriefListUI';
import './BriefListUI.css';
import { Icon, Segment, Label, Popup, Header, Grid, Button, Dropdown } from 'semantic-ui-react';
export const convertMyProjectsToBriefList = (myProjects, onProjectClicked) => {
    const myProjectBriefElements = myProjects.map((project) => {
        const icon = project.isActive ? <Icon name="lock open" style={{color: "rgb(196, 126, 5)"}}/> : <Icon name="lock" style={{color: "rgb(196, 126, 5)"}}/>;
        const title = project.courseCode + '/' + project.projectName;
        return (
            <p className={'BriefListElements'}>
            <Link style={{fontWeight: "bold"}} onClick={() => onProjectClicked(project.projectId)}>{icon}{title}</Link>
            </p>
        );
    });

    return <div>{myProjectBriefElements}</div>;
};

export const convertInstructedCoursesToBriefList = (instructedCourses, onCourseClicked) => {
    const instructedCourseBriefElements = instructedCourses.map((course) => {
        const icon = course.isActive ? <Icon name="lock open" style={{color: "rgb(196, 126, 5)"}}/> : <Icon name="lock" style={{color: "rgb(196, 126, 5)"}}/>;
        const title = course.courseCode;
        return (<p className={'BriefListElements'}><Link  style={{fontWeight: "bold"}} onClick={() => onCourseClicked(course.courseId)}>{icon}{title}</Link></p>);
    });

    return <div>{instructedCourseBriefElements}</div>;
};

export const convertUpcomingAssignmentsToBriefList = (upcomingAssignments, onAssignmentClicked) => {
    const upcomingAssignmentsBriefElements = upcomingAssignments ? (
        upcomingAssignments.map((assignment) => {
            const title = assignment.courseCode + '/' + assignment.assignmentName;
            return (
                <Segment>
                    <Link onClick={() => onAssignmentClicked(assignment.projectId, assignment.submissionId)}
                    >{title}</Link>
                    <div align="right" className="DueDate">{assignment.dueDate}</div>
                </Segment>
            );
        })
    ) : (
        <div></div>
    );

    return (
        <Segment.Group raised>
            <Label attached='top' style={{backgroundColor: "rgb(33,133,208)", color: "white", textAlign: "center"}}>Upcoming Assignments</Label>
            {upcomingAssignmentsBriefElements}
        </Segment.Group>
        );
};

export const convertNotGradedAssignmentsToBriefList = (notGradedAssignments, onAssignmentClicked) => {
    const notGradedAssignmentsBriefElements = notGradedAssignments ? (notGradedAssignments.map((assignment) => {
        const title = assignment.courseCode + '/' + assignment.assignmentName;
        return (
            <Segment>
                <Link onClick={() => onAssignmentClicked(assignment.courseId, assignment.assignmentId)}
                >{title}</Link>
                <div align="right" className="DueDate">{assignment.dueDate}</div>
            </Segment>
        );
    })) : (
        <div></div>
    );
    
    return (
        <Segment.Group raised>
            <Label attached='top' style={{backgroundColor: "rgb(219,40,40)", color: "white", textAlign: "center"}}>Not Graded Assignments</Label>
            {notGradedAssignmentsBriefElements}
        </Segment.Group>
        );
};

export const convertMembersToMemberElement = (members, onUserClicked, title="") => {
    const convertedList = members?.map((member) => {
        
        return (
            <p ><Link style={{fontWeight: "bold"}} onClick={() => onUserClicked(member.userId)}>
                 {member.name}
            </Link></p>
            )
    });
    return (<div><h4 style={{ marginLeft: '20px' }}>{title}</h4>{convertedList}</div>);
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
