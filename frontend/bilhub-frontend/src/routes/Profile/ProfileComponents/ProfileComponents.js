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
                            onUserClicked={(userId) => props.onUserClicked(userId)}
                            onCourseClicked={() => props.onCourseClicked(course.courseId)}
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
                            onUserClicked={(userId) => props.onUserClicked(userId)}
                            onProjectClicked={() => props.onProjectClicked(project.projectId)}
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
                    <div onClick={props.onProjectClicked}>{props.title}</div>
                </Card.Header>
                <Card.Description>{convertMembersToMemberElement(props.members, props.onUserClicked)}</Card.Description>
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
                    <div onClick={props.onCourseClicked}>{props.title}</div>
                </Card.Header>
                <Card.Description>
                    <h4>TAs</h4>
                    {convertMembersToMemberElement(props.TAs, props.onUserClicked)}
                </Card.Description>
                <Card.Description>
                    <h4>Instructors</h4>
                    {convertMembersToMemberElement(props.instructors, props.onUserClicked)}
                </Card.Description>
            </Card.Content>
        </Card>
    );
};
