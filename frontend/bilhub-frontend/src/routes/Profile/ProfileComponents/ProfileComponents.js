import React from 'react';
import { Segment, Icon, Card, Grid } from 'semantic-ui-react';

import { convertMembersToMemberElement, Tab } from '../../../components';

export const ProfilePrompt = (props) => {
    return (
        <Segment style={{ boxShadow: 'none', border: '0' }}>
            <h2 className="ui center aligned icon header">
                <Icon circular name="user" />
                {props.name}
            </h2>
            <p align="center">{props.email}</p>
            <h4 style={{ marginLeft: '20px' }}>Information</h4>
            <p className="InformationText" style={{ display: 'inline-block' }}>
                {props.informationElement}
            </p>
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
                            information={course.information}
                        />
                    );
                })
            ) : (
                <div>No Instructed Courses</div>
            ),
        },
    ];

    if (props.userType !== 'Instructor') {
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
                            information={project.information}
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
        <Card className="ProjectCardElement" fluid style={{ width: '95%' }}>
            <Card.Content>
                <Card.Header className="clickableChangeColor">
                    <div onClick={props.onProjectClicked}>{props.title}</div>
                </Card.Header>
                <Card.Description>
                    <Grid columns={2} divided>
                        <Grid.Column>
                            {convertMembersToMemberElement(
                                props.members.slice(0, Math.ceil(props.members.length / 2)),
                                props.onUserClicked
                            )}
                        </Grid.Column>
                        <Grid.Column>
                            {convertMembersToMemberElement(
                                props.members.slice(Math.ceil(props.members.length / 2)),
                                props.onUserClicked
                            )}
                        </Grid.Column>
                    </Grid>
                    <p style={{ marginTop: '15px' }}>{props.information}</p>
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
        <Card className="ProjectCardElement" fluid style={{ width: '95%' }}>
            <Card.Content>
                <Card.Header className="clickableChangeColor">
                    <div onClick={props.onCourseClicked}>{props.title}</div>
                </Card.Header>
                <Card.Description>
                    <Grid columns={2} divided>
                        <Grid.Column>
                            <label style={{ fontSize: '14px', fontWeight: 'bold' }}>Instructors</label>
                            {convertMembersToMemberElement(props.instructors, props.onUserClicked)}
                        </Grid.Column>
                        <Grid.Column>
                            <label style={{ fontSize: '14px', fontWeight: 'bold' }}>TAs</label>
                            {convertMembersToMemberElement(props.TAs, props.onUserClicked)}
                        </Grid.Column>
                    </Grid>
                    <p style={{ marginTop: '15px' }}>{props.information}</p>
                </Card.Description>
            </Card.Content>
        </Card>
    );
};
