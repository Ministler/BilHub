import React from 'react';
import { Segment, Icon, Card } from 'semantic-ui-react';

import { convertMembersToMemberElement, Tab } from '../../../components';

export const ProfilePrompt = (props) => {
    return (
        <Segment>
            <h2 className="ui center aligned icon header">
                <Icon circular name="user" size="Massive" />
                {props.name}
            </h2>
            <p align="center">{props.email}</p>
            <h4 style={{ marginLeft: '20px' }}>Information</h4>
            {props.informationElement}
            {props.icon}
        </Segment>
    );
};

export const TabExampleSecondaryPointing = (props) => {
    const panes = [
        {
            title: 'Instructed Courses',
            content: props.courses ? (
                props.courses.map((course) => {
                    return (
                        <CourseCardElement
                            title={course.courseName}
                            TAs={course.TAs}
                            instructors={course.instructors}
                            onMemberClicked={(userId) => props.onMemberClicked(userId)}
                            onCourseTitleClicked={() => props.onCourseTitleClicked(course.courseId)}
                        />
                    );
                })
            ) : (
                <div>No Instructed Courses</div>
            ),
        },
    ];

    if (props.userType !== 'instructor') {
        panes.unshift({
            title: 'Projects',
            content: props.projects ? (
                props.projects.map((project) => {
                    return (
                        <ProjectCardElement
                            title={project.courseName + ' / ' + project.groupName}
                            members={project.groupMembers}
                            peerGrade={project.peerGrade}
                            projectGrade={project.projectGrade}
                            instructor={project.instructor}
                            onMemberClicked={(userId) => props.onMemberClicked(userId)}
                            onProjectTitleClicked={() => props.onProjectTitleClicked(project.projectId)}
                        />
                    );
                })
            ) : (
                <div>No Projects Yet</div>
            ),
        });
    }

    return <Tab tabPanes={panes} />;
};

export const ProjectCardElement = (props) => {
    return (
        <Card className="ProjectCardElement">
            <Card.Content>
                <Card.Header>
                    <div onClick={props.onProjectTitleClicked}>{props.title}</div>
                </Card.Header>
                <Card.Description>
                    {convertMembersToMemberElement(props.members, props.onMemberClicked)}
                </Card.Description>
            </Card.Content>
            <Card.Content className="ProjectCardExtra">
                <div className="ProjectGrade">
                    Peer Grade: {props.peerGrade} Project Grade: {props.projectGrade}
                </div>
            </Card.Content>
        </Card>
    );
};

export const CourseCardElement = (props) => {
    return (
        <Card className="ProjectCardElement">
            <Card.Content>
                <Card.Header>
                    <div onClick={props.onCourseTitleClicked}>{props.title}</div>
                </Card.Header>
                <Card.Description>
                    <h4>TAs</h4>
                    {convertMembersToMemberElement(props.TAs, props.onMemberClicked)}
                </Card.Description>
                <Card.Description>
                    <h4>Instructors</h4>
                    {convertMembersToMemberElement(props.instructors, props.onMemberClicked)}
                </Card.Description>
            </Card.Content>
        </Card>
    );
};
