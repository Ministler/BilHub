import React from 'react';

import {
    BriefList,
    TitledIconedBriefElement,
    TitledDatedBriefElement,
    MemberBriefElement,
    SubmissionBriefElement,
    GroupBriefElement,
} from './BriefListUI';
import { Icon, Segment, Label } from 'semantic-ui-react';
export const convertMyProjectsToBriefList = (myProjects, onProjectClicked) => {
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
