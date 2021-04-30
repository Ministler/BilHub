import React, { Component } from 'react';
import { Segment, TextArea, Icon, Button, Dropdown } from 'semantic-ui-react';
import { connect } from 'react-redux';

import './Course.css';
import {
    InformationSection,
    NewAssignmentModal,
    EditAssignmentModal,
    DeleteAssignmentModal,
    GroupsTab,
} from './CourseComponents';
import { Tab, convertAssignmentsToAssignmentList, getCourseStatistics } from '../../components';
import { CourseAssignment } from './CourseAssignment';

class Course extends Component {
    constructor(props) {
        super(props);
        this.state = {
            courseInformation: null,
            informationEditMode: false,
            newInformation: '',

            assignments: null,

            isNewAssignmentOpen: false,
            isEditAssignmentOpen: false,
            isDeleteAssignmentOpen: false,

            currentSection: 1,
        };
    }

    changeCourseInformation = (newInformation) => {
        const request = {
            newInformation: newInformation,
            courseId: this.state.courseInformation?.courseId,
        };

        console.log(request);
    };

    onAssignmentFileClicked = () => {
        console.log('file');
    };

    onNewAssignmentModalClosed = (isSuccess) => {
        this.setState({
            isNewAssignmentOpen: false,
        });
    };
    onEditAssignmentModalClosed = (isSuccess) => {
        this.setState({
            isEditAssignmentOpen: false,
        });
    };
    onDeleteAssignmentModalClosed = (isSuccess) => {
        this.setState({
            isDeleteAssignmentOpen: false,
        });
    };

    componentDidMount() {
        this.setState({
            groups: dummyGroups,
            courseInformation: dummyCourseInformation,
            newInformation: dummyCourseInformation.information,
            assignments: dummyCourseAssignments,
        });
    }

    onNewAssignmentModalOpened = () => {
        this.setState({
            isNewAssignmentOpen: true,
        });
    };

    onEditAssignmentModalOpened = () => {
        this.setState({
            isEditAssignmentOpen: true,
        });
    };

    onDeleteAssignmentModalOpened = () => {
        this.setState({
            isDeleteAssignmentOpen: true,
        });
    };

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

    onAssignmentClicked = (courseId, assignmentId) => {
        this.props.history.push('/course/' + this.props.match.params.courseId + '/assignment/' + assignmentId);
    };

    getCourseInformationItem = () => {
        let courseInformationElement = this.state.courseInformation?.information;
        if (this.state.informationEditMode) {
            courseInformationElement = (
                <TextArea
                    className="InformationText"
                    onChange={(e) => this.onInputChanged(e, 'newInformation')}
                    value={this.state.newInformation}
                />
            );
        }
        return courseInformationElement;
    };

    getInformationEditIcon = () => {
        let informationEditIcon = null;
        if (this.state.courseInformation?.isCourseActive && this.state.courseInformation?.isTAorInstructorOfCourse) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeCourseInformation(this.state.newInformation);
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'check'}
                    color="blue"
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'edit'}
                    color="blue"
                />
            );
        }

        return informationEditIcon;
    };

    onCourseSettingsClicked = () => {
        this.props.history.push('/course/' + this.props.match.params.courseId + '/settings');
    };

    onSectionChanged = (dropdownValues) => {
        this.setState({
            currentSection: dropdownValues.value,
        });
    };

    getCourseSettingsIcon = () => {
        let icon = null;
        if (this.state.courseInformation?.isCourseActive && this.state.courseInformation?.isTAorInstructorOfCourse) {
            icon = <Icon name="setting" onClick={this.onCourseSettingsClicked} color="grey" />;
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

    getDropdownForSections = () => {
        if (this.state.courseInformation?.numberOfSections) {
            const sectionOptions = [];
            for (let i = 1; i <= this.state.courseInformation?.numberOfSections; i++) {
                sectionOptions.push({
                    key: i,
                    text: 'Section ' + i,
                    value: i,
                });
            }
            return (
                <Dropdown
                    defaultValue={0}
                    fluid
                    selection
                    options={sectionOptions}
                    onChange={(e, dropdownValues) => this.onSectionChanged(dropdownValues)}
                />
            );
        }

        return null;
    };

    getGroupsPane = () => {
        return {
            title: 'Groups',
            content: (
                <>
                    {this.getDropdownForSections()}
                    {/* {this.state.groups &&
                    (1 <= this.state.currentSection ||
                        this.state.currentSection <= this.state.courseInformation.numberOfSections) ? (
                        <GroupsTab
                            groupsFormed={this.state.groups['section' + this.state.currentSection].formed}
                            groupsUnformed={this.state.groups['section' + this.state.currentSection].unformed}
                        />
                    ) : null} */}
                </>
            ),
        };
    };

    getStatisticsPane = () => {
        return {
            title: 'Statistics',
            content: <>{getCourseStatistics(dummyCourseGrades, dummyFinalGrades)} </>,
        };
    };

    getNewAssignmentButton = () => {
        let button = null;
        if (this.state.courseInformation?.isCourseActive && this.state.courseInformation?.isTAorInstructorOfCourse) {
            button = (
                <Button
                    content="New Assignment"
                    labelPosition="right"
                    icon="add"
                    primary
                    style={{ marginTop: '20px' }}
                    onClick={this.onNewAssignmentModalOpened}
                />
            );
        }
        return button;
    };

    getAssignmentPane = () => {
        return {
            title: 'Assignments',
            content: this.state.assignments ? (
                <>
                    {convertAssignmentsToAssignmentList(
                        this.state.assignments,
                        this.onAssignmentClicked,
                        null,
                        this.onAssignmentFileClicked,
                        this.getAssignmentControlIcons()
                    )}
                    {this.getNewAssignmentButton()}
                </>
            ) : (
                <>
                    <div>No Assignments</div>
                    {this.getNewAssignmentButton()}
                </>
            ),
        };
    };

    getAssignmentControlIcons = () => {
        let controlIcons = null;
        if (this.state.courseInformation?.isCourseActive && this.state.courseInformation?.isTAorInstructorOfCourse) {
            controlIcons = (
                <>
                    <Icon
                        name="close"
                        color="red"
                        size="small"
                        style={{ float: 'right' }}
                        onClick={this.onDeleteAssignmentModalOpened}
                    />
                    <Icon
                        name="edit"
                        color="blue"
                        size="small"
                        style={{ float: 'right' }}
                        onClick={this.onEditAssignmentModalOpened}
                    />
                </>
            );
        }

        return controlIcons;
    };

    getCoursePanes = () => {
        return [this.getGroupsPane(), this.getStatisticsPane(), this.getAssignmentPane()];
    };

    getAssignmentPage = () => {
        return (
            <CourseAssignment
                isCourseActive={this.state.courseInformation?.isCourseActive}
                courseName={this.state.courseInformation?.courseName}
                onEditAssignmentModalOpened={this.onEditAssignmentModalClosed}
                onEditAssignmentModalClosed={this.onEditAssignmentModalClosed}
                onDeleteAssignmentModalOpened={this.onDeleteAssignmentModalOpened}
                onDeleteAssignmentModalClosed={this.onDeleteAssignmentModalClosed}
            />
        );
    };

    getModals = () => {
        return (
            <>
                <NewAssignmentModal
                    onClosed={this.onNewAssignmentModalClosed}
                    isOpen={this.state.isNewAssignmentOpen}
                />
                <EditAssignmentModal
                    onClosed={this.onEditAssignmentModalClosed}
                    isOpen={this.state.isEditAssignmentOpen}
                />
                <DeleteAssignmentModal
                    onClosed={this.onDeleteAssignmentModalClosed}
                    isOpen={this.state.isDeleteAssignmentOpen}
                />
            </>
        );
    };

    render() {
        return (
            <>
                <div class="ui centered grid">
                    <div class="row">
                        <div class="four wide column">
                            <Segment>{this.getInformationSection()}</Segment>
                        </div>
                        <div class="twelve wide column">
                            {this.props.match.params.assignmentId ? (
                                this.getAssignmentPage()
                            ) : (
                                <Tab tabPanes={this.getCoursePanes()} />
                            )}
                        </div>
                    </div>
                </div>
                {this.getModals()}
            </>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        token: state.token,
        userId: state.userId,
        userName: state.name,
        userType: state.userType,
    };
};

export default connect(mapStateToProps)(Course);

const dummyGroups = {
    section1: {
        formed: [
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                isUserInGroup: true,
            },
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
            },
        ],
        unformed: [
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                isUserInGroup: true,
            },
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
            },
        ],
    },
    section2: {
        formed: [
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                isUserInGroup: true,
            },
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
            },
        ],
        unformed: [
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                isUserInGroup: true,
            },
            {
                members: [
                    {
                        name: 'Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: 'Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: 'Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
            },
        ],
    },
};

const dummyCourseInformation = {
    courseName: 'CS319-2021Spring',
    description: 'Object-Oriented Software Engineering',
    isCourseActive: true,
    instructors: [
        {
            name: 'Eray Tüzün',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            userId: 2,
        },
    ],
    TAs: [
        {
            name: 'Erdem Tuna',
            userId: 1,
        },
        {
            name: 'Kraliçe Irmak',
            userId: 2,
        },
    ],
    information:
        'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Odio, et assumenda fugiat repudiandae doloribus eaque at possimus tenetur cum ratione, non voluptatibus? Provident nam cum et cupiditate corporis earum vel ut? Illum beatae molestiae praesentium cumque sapiente, quasi neque consequatur distinctio iste possimus in dolor. Expedita rem totam ex distinctio!',
    isTAorInstructorOfCourse: true,
    usersSection: 4,
    isUserInFormedGroup: true,
    isLocked: true,
    formationDate: '15/24/2020',
    numberOfSections: 5,
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
    },
    {
        title: 'Design Report',
        caption:
            'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Pariatur optio dolores modi illo, soluta nesciunt? Explicabo dicta ad nulla ea.',
        assignmentId: 2,
        publisher: 'Erdem Tuna',
        publishmentDate: '12 March 2021 12:00',
        dueDate: '12 April 2021 12:00',
    },
    {
        title: 'Design Report',
        caption:
            'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Pariatur optio dolores modi illo, soluta nesciunt? Explicabo dicta ad nulla ea.',
        assignmentId: 2,
        publisher: 'Erdem Tuna',
        publishmentDate: '12 March 2021 12:00',
        dueDate: '12 April 2021 12:00',
    },
    {
        title: 'Design Report',
        caption:
            'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Pariatur optio dolores modi illo, soluta nesciunt? Explicabo dicta ad nulla ea.',
        assignmentId: 2,
        publisher: 'Erdem Tuna',
        publishmentDate: '12 March 2021 12:00',
        dueDate: '12 April 2021 12:00',
    },
];

const dummyCourseGrades = {
    graders: ['Eray Tüzün', 'Alper Sarıkan', 'Erdem Tuna', 'Kraliçe Irmak', 'Students'],
    groups: [
        { name: 'BilHub', grades: [99, 98, 97, 10, 89] },
        { name: 'BilCalendar', grades: [75, 45, 23, 10, 89] },
        { name: 'CS315Odevi', grades: [38, 98, 97, 1, 43] },
        { name: 'Website', grades: [46, 87, 24, 10, 94] },
    ],
};

const dummyFinalGrades = [
    { group: 'BilHub', grade: 90 },
    { group: 'BilHub2', grade: 20 },
    { group: 'BilHub3', grade: 80 },
    { group: 'BilHubNot', grade: 78 },
    { group: 'OZCO1000', grade: 80 },
    { group: 'BilCalendar', grade: 78 },
    { group: 'Yusuf Keke', grade: 80 },
    { group: 'Website', grade: 78 },
];
