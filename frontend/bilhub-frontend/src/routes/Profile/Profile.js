import React, { Component } from 'react';
import { TextArea, Icon, Grid } from 'semantic-ui-react';
import { connect } from 'react-redux';

import { getUserGroupsRequest, getInstructedCoursesRequest } from '../../API';
import './Profile.css';
import { TabExampleSecondaryPointing, ProfilePrompt } from './ProfileComponents';

class Profile extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userInformation: null,
            projects: null,
            courses: null,

            informationEditMode: false,
            newInformation: '',
        };
        console.log(this.props);
    }

    changeUserInformation = (newInformation) => {
        const request = {
            newInformation: newInformation,
            userId: this.props.userId,
        };

        console.log(request);
    };

    componentDidMount() {
        getUserGroupsRequest(this.props.userId).then((response) => {
            if (!response.data.success) return;

            const projects = [];
            for (let group of response.data.data) {
                const groupMembers = [];
                for (let member of group.groupMembers) {
                    groupMembers.push({
                        userId: member.id,
                        name: member.name,
                    });
                }
                console.log(group);
                projects.push({
                    courseName: group.affiliatedCourse.name,
                    groupName: group.name,
                    projectId: group.id,
                    isActive: group.isActive,
                    groupMembers: groupMembers,
                    information: group.projectInformation,
                    //peerGrade:,
                    //projectGrade:,
                });
            }

            this.setState({
                projects: projects,
            });
        });

        getInstructedCoursesRequest(this.props.userId).then((response) => {
            if (!response.data.success) return;

            const courses = [];
            for (let course of response.data.data) {
                const courseInstructors = [];
                const courseTAs = [];
                for (let member of course.instructors) {
                    if (member.userType === 'Instructor') {
                        courseInstructors.push({
                            name: member.name,
                            userId: member.id,
                        });
                    } else {
                        courseTAs.push({
                            name: member.name,
                            userId: member.id,
                        });
                    }
                }

                courses.push({
                    courseName: course?.name + '-' + course?.year + course?.courseSemester,
                    courseId: course.id,
                    TAs: courseTAs,
                    instructors: courseInstructors,
                    //information: course.information,
                });
            }

            this.setState({
                courses: courses,
            });
        });

        this.setState({
            userInformation: dummyUser.userInformation,
            newInformation: dummyUser.userInformation,
        });
    }

    onNewInformationChanged = (e) => {
        e.preventDefault();
        this.setState({
            newInformation: e.target.value,
        });
    };

    onInformationEditModeToggled = () => {
        this.setState((prevState) => {
            return {
                informationEditMode: !prevState['informationEditMode'],
            };
        });
    };

    onUserClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    onProjectClicked = (projectId) => {
        this.props.history.push('/project/' + projectId);
    };

    onCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    getInformationElement = () => {
        let groupInformationElement = <p>{this.state.userInformation}</p>;
        if (this.state.informationEditMode) {
            groupInformationElement = (
                <TextArea onChange={(e) => this.onNewInformationChanged(e)} value={this.state.newInformation} />
            );
        }
        return groupInformationElement;
    };

    getEditInformationIcon = () => {
        let informationEditIcon = null;
        if (!this.props.match.params.id || this.props.match.params.id === this.props.userId) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeUserInformation(this.state.newInformation);
                        this.onInformationEditModeToggled();
                    }}
                    name={'check'}
                    color="blue"
                    style={{ float: 'right', marginTop: '-10px' }}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onInformationEditModeToggled();
                    }}
                    name={'edit'}
                    color="blue"
                    style={{ float: 'right', marginTop: '-15px' }}
                />
            );
        }
        return informationEditIcon;
    };

    render() {
        return (
            <div class="ui centered grid">
                <Grid.Row divided>
                    <div class="four wide column">
                        <ProfilePrompt
                            name={this.props.name}
                            email={this.props.email}
                            informationElement={this.getInformationElement()}
                            icon={this.getEditInformationIcon()}
                        />
                    </div>
                    <div class="twelve wide column">
                        <TabExampleSecondaryPointing
                            userType={this.props.userType}
                            projects={this.state.projects}
                            courses={this.state.courses}
                            onUserClicked={(userId) => this.onUserClicked(userId)}
                            onProjectClicked={(projectId) => this.onProjectClicked(projectId)}
                            onCourseClicked={(courseId) => this.onCourseClicked(courseId)}
                        />
                    </div>
                </Grid.Row>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        userType: state.userType,
        userId: state.userId,
        email: state.email,
        name: state.name,
    };
};

export default connect(mapStateToProps)(Profile);

const dummyUser = {
    userInformation:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Delectus id aspernatur ea sitanimi, ab qui! Ea beatae dolorum inventore cum quibusdam placeat quisquam itaque, odioquasi numquam maiores quidem illum odit commodi dicta animi voluptas tempora? Adipisci maiores inventore minus provident quas minima itaque saepe et labore, ut sequi!',
};
