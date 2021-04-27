import React, { Component } from 'react';
import { Segment, TextArea, Icon } from 'semantic-ui-react';

import './Course.css';
import { InformationSection } from './CourseComponents';
import { GroupsTab } from './GroupsTab';
import { Tab, convertAssignmentsToAssignmentList } from '../../components';

export class Course extends Component {
    constructor(props) {
        super(props);
        this.state = {
            courseInformation: null,
            informationEditMode: false,
            newInformation: '',
            assignments: null,
        };
    }

    componentDidMount() {
        this.setState({
            courseInformation: dummyCourseInformation,
            newInformation: dummyCourseInformation.information,
            assignments: dummyCourseAssignments,
        });
    }

    onUserClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    onInputChanged = (e, stateName) => {
        e.preventDefault();
        this.setState({
            [stateName]: e.target.value,
        });
    };

    onEditModeToggled = (editMode) => {
        this.setState((prevState) => {
            return {
                [editMode]: !prevState[editMode],
            };
        });
    };

    changeCourseInformation = (newInformation) => {
        let courseInformation = { ...this.state.courseInformation };
        courseInformation.information = newInformation;
        this.setState({
            courseInformation: courseInformation,
        });
    };

    getCourseInformationItem = () => {
        let courseInformationElement = this.state.courseInformation?.information;
        if (this.state.informationEditMode) {
            courseInformationElement = (
                <TextArea
                    onChange={(e) => this.onInputChanged(e, 'newInformation')}
                    value={this.state.newInformation}
                />
            );
        }
        return courseInformationElement;
    };

    getInformationEditIcon = () => {
        let informationEditIcon = null;
        if (this.state.courseInformation?.isCourseActive && this.state.courseInformation?.isUserInstructorOfCourse) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeCourseInformation(this.state.newInformation);
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'check'}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'edit'}
                />
            );
        }

        return informationEditIcon;
    };

    onCourseSettingsClicked = () => {
        this.props.history.push('/course/' + this.props.match.params.courseId + '/settings');
    };

    getCourseSettingsIcon = () => {
        let icon = null;
        if (this.state.courseInformation?.isUserTAOfCourse) {
            icon = <Icon name="settings" onClick={this.onCourseSettingsClicked} />;
        }
        return icon;
    };

    getInformationSection = () => {
        return (
            <InformationSection
                courseName={this.state.courseInformation?.courseName}
                description={this.state.courseInformation?.description}
                courseSettingsIcon={this.getCourseSettingsIcon()}
                instructors={this.state.courseInformation?.instructors}
                TAs={this.state.courseInformation?.TAs}
                informationEditIcon={this.getInformationEditIcon()}
                informationElement={this.getCourseInformationItem()}
                onUserClicked={this.onUserClicked}
            />
        );
    };

    getGroupsPane = () => {
        return {
            title: 'Groups',
            content: <GroupsTab groupsFormed={dummyGroupsFormed} groupsUnformed={dummyGroupsUnformed} />,
        };
    };

    getStatisticsPane = () => {
        return {
            title: 'Statistics',
            content: <></>,
        };
    };

    getAssignmentPane = () => {
        return {
            title: 'Assignments',
            content: this.state.assignments ? (
                convertAssignmentsToAssignmentList(
                    this.state.assignments,
                    this.onAssignmentClicked,
                    this.onAssignmentFileClicked,
                    this.assignmentIcons
                )
            ) : (
                <div>No Assignments</div>
            ),
        };
    };

    getAssignmentControlIcons = () => {};

    getCoursePanes = () => {
        return [this.getGroupsPane(), this.getStatisticsPane(), this.getAssignmentPane()];
    };

    render() {
        return (
            <div class="ui centered grid">
                <div class="row">
                    <div class="four wide column">
                        <Segment>{this.getInformationSection()}</Segment>
                    </div>
                    <div class="twelve wide column">
                        <Tab tabPanes={this.getCoursePanes()} />
                    </div>
                </div>
            </div>
        );
    }
}

const dummyGroupsFormed = [
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
];

const dummyGroupsUnformed = [
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Yusuf Uyar', 'Barış Ogün Yörük', 'Oğuzhan Özçelik'],
    ['Yusuf Uyar', 'Barış Ogün Yörük', 'Oğuzhan Özçelik'],
    ['Keke. One', 'Keke. Three', 'Keke. Five'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Mr. One'],
    ['Keke. Five'],
    ['Mr. One'],
    ['Mr. One'],
    ['Mr. One'],
    ['Dummy. Two'],
    ['Dummy. Two'],
    ['Keke. Five'],
];

const dummyCourseInformation = {
    courseName: 'CS319-2021Spring',
    description: 'Object-Oriented Software Engineering',
    isCourseActive: true,
    instructors: [
        {
            name: 'Eray Tüzün',
            information: 'eraytuzun@gmail.com',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            information: 'alpersarikan@gmail.com',
            userId: 2,
        },
    ],
    TAs: [
        {
            name: 'Erdem Tuna',
            information: 'erdemtuan@gmail.com',
            userId: 1,
        },
        {
            name: 'Kraliçe Irmak',
            information: 'kraliceirmak@gmail.com',
            userId: 2,
        },
    ],
    information:
        'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Odio, et assumenda fugiat repudiandae doloribus eaque at possimus tenetur cum ratione, non voluptatibus? Provident nam cum et cupiditate corporis earum vel ut? Illum beatae molestiae praesentium cumque sapiente, quasi neque consequatur distinctio iste possimus in dolor. Expedita rem totam ex distinctio!',
    isUserInstructorOfCourse: true,
    isUserTAOfCourse: true,
};

const dummyCourseAssignments = [
    {
        title: 'Analysis Report',
        caption:
            'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Pariatur optio dolores modi illo, soluta nesciunt? Explicabo dicta ad nulla ea.',
        assignmentId: 1,
        publisher: 'Elgun Jabrayilzade',
        file: 'file',
        publishmentDate: '12 March 2021 12:00',
        dueDate: '12 March 2021 12:00',
        isUserAuthForAssignment: true,
    },
    {
        title: 'Design Report',
        caption:
            'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Pariatur optio dolores modi illo, soluta nesciunt? Explicabo dicta ad nulla ea.',
        assignmentId: 2,
        publisher: 'Erdem Tuna',
        publishmentDate: '12 March 2021 12:00',
        dueDate: '12 April 2021 12:00',
        isUserAuthForAssignment: false,
    },
];
