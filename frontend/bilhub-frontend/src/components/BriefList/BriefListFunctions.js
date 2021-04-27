import React from 'react';

import {
    BriefList,
    TitledIconedBriefElement,
    TitledDatedBriefElement,
    MemberBriefElement,
    GroupBriefElement,
} from './BriefListUI';
import { Icon, Segment, Label } from 'semantic-ui-react';

export const convertMyProjectsToBriefList = (myProjects, onMyProjectClicked) => {
    const myProjectBriefElements = myProjects.map((project) => {
        const icon = project.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
        const title = project.courseCode + '/' + project.projectName;
        return (
            <TitledIconedBriefElement icon={icon} title={title} onClick={() => onMyProjectClicked(project.projectId)} />
        );
    });

    return <BriefList title={'My Projects'}>{myProjectBriefElements}</BriefList>;
};

export const convertInstructedCoursesToBriefList = (instructedCourses, onInstructedCourseClicked) => {
    const instructedCourseBriefElements = instructedCourses.map((course) => {
        const icon = course.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
        const title = course.courseCode;
        return (
            <TitledIconedBriefElement
                icon={icon}
                title={title}
                onClick={() => onInstructedCourseClicked(course.courseId)}
            />
        );
    });

    return <BriefList title={'Instructed Courses'}>{instructedCourseBriefElements}</BriefList>;
};

export const convertUpcomingAssignmentsToBriefList = (upcomingAssignments, onUpcomingAssignmentClicked) => {
    const upcomingAssignmentsBriefElements = upcomingAssignments ? (
        upcomingAssignments.map((assignment) => {
            const title = assignment.courseCode + '/' + assignment.assignmentName;
            return (
                <TitledDatedBriefElement
                    title={title}
                    date={assignment.dueDate}
                    onClick={() => onUpcomingAssignmentClicked(assignment.projectId, assignment.submissionPageId)}
                />
            );
        })
    ) : (
        <div>You Have No Upcoming Assignments</div>
    );

    return <BriefList title={'Upcoming Assignments'}>{upcomingAssignmentsBriefElements}</BriefList>;
};

export const convertNotGradedAssignmentsToBriefList = (notGradedAssignments, onNotGradedAssignmentClicked) => {
    const notGradedAssignmentsBriefElements = notGradedAssignments.map((assignment) => {
        const title = assignment.courseCode + '/' + assignment.assignmentName;
        return (
            <TitledDatedBriefElement
                title={title}
                date={assignment.dueDate}
                onClick={() => onNotGradedAssignmentClicked(assignment.courseId, assignment.asignmentId)}
            />
        );
    });

    return <BriefList title={'Not Graded Assignments'}>{notGradedAssignmentsBriefElements}</BriefList>;
};

export const convertMembersToMemberElement = (members, onMemberClicked) => {
    return members?.map((member) => {
        return <MemberBriefElement onClick={() => onMemberClicked(member.userId)} member={member} />;
    });
};

export const convertUnformedGroupsToBriefList = (groups) => {
    return groups?.map((group) => {
        return (
            <Segment className="clickableHighlightBack" style={{ width: '15%', float: 'left', margin: '10px' }}>
                <GroupBriefElement group={group.members} />
            </Segment>
        );
    });
};

export const convertFormedGroupsToBriefList = (groups) => {
    return groups?.map((group) => {
        return (
            <Segment className="clickableHighlightBack" style={{ width: '15%', float: 'left', margin: '10px' }}>
                <Label style={{ textAlign: 'center' }} attached="top">
                    {group.name}
                </Label>
                <GroupBriefElement group={group.members} />
            </Segment>
        );
    });
};
