import React, { Component } from 'react';
import { Segment, TextArea, Icon, Button, Dropdown, Grid } from 'semantic-ui-react';
import { connect } from 'react-redux';

import './Course.css';
import {
    InformationSection,
    NewAssignmentModal,
    EditAssignmentModal,
    DeleteAssignmentModal,
    GroupsTab,
    SendRequestModal,
    UnformedGroupModal,
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

            isSendRequestModalOpen: false,
            isUnformedGroupModalOpen: false,
            currentGroupId: null,
            currentVoteStatus: null,
            currentMembers: null,
            currentIsUserReady: null,
            currentIsFormable: null,
            currentMessage: '',

            currentSection: 0,
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

    onSendRequestModalClosed = (isSuccess, type) => {
        this.setState({
            isSendRequestModalOpen: false,
            currentMessage: '',
        });
        if (!isSuccess) return;

        const request = {
            groupId: this.state.currentGroupId,
            message: this.state.currentMessage,
            type: type, // merge or join
        };

        console.log(request);
    };

    onUnformedGroupModalClosed = (e, isSuccess, type) => {
        this.setState({
            isUnformedGroupModalOpen: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        if (type === 'exit') {
            request = {
                groupId: this.state.currentGroupId,
                exit: true,
            };
        } else if (type === 'update') {
            request = {
                groupId: this.state.currentGroupId,
                isUserReady: e.target.isReady.checked,
            };
        }

        console.log(request);
    };

    componentDidMount() {
        this.setState({
            groups: dummyGroupsLocked,
            courseInformation: dummyCourseInformation,
            newInformation: dummyCourseInformation.information,
            assignments: dummyCourseAssignments,
            currentSection: dummyCourseInformation.currentUserSection
                ? dummyCourseInformation.currentUserSection - 1
                : 0,
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

    onSendRequestModalOpened = (groupId, members) => {
        this.setState({
            currentGroupId: groupId,
            isSendRequestModalOpen: true,
            currentMembers: members,
        });
    };

    onUnformedGroupModalOpened = (groupId, voteStatus, members, isUserReady, isFormable) => {
        this.setState({
            currentGroupId: groupId,
            isUnformedGroupModalOpen: true,
            currentVoteStatus: voteStatus,
            currentMembers: members,
            currentIsUserReady: isUserReady,
            currentIsFormable: isFormable,
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
                    style={{ float: 'right', marginTop: '-10px' }}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'edit'}
                    color="blue"
                    style={{ float: 'right', marginTop: '-15px' }}
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

    onTextChanged = (e) => {
        this.setState({
            currentMessage: e.target.value,
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
            for (let i = 0; i < this.state.courseInformation?.numberOfSections; i++) {
                sectionOptions.push({
                    key: i,
                    text: 'Section ' + (i + 1),
                    value: i,
                });
            }
            return (
                <Dropdown
                    fluid
                    selection
                    options={sectionOptions}
                    value={this.state.currentSection}
                    onChange={(e, dropdownValues) => this.onSectionChanged(dropdownValues)}
                />
            );
        }

        return null;
    };

    onGroupClicked = (projectId) => {
        this.props.history.push('/project/' + projectId);
    };

    getGroupsPane = () => {
        let groupsTab = null;

        if (this.state.courseInformation?.isLocked) {
            groupsTab = this.state.groups ? (
                <GroupsTab
                    groupsFormed={this.state.groups[this.state.currentSection]}
                    isLocked={this.state.courseInformation?.isLocked}
                    onGroupClicked={this.onGroupClicked}
                />
            ) : null;
        } else {
            groupsTab = this.state.groups ? (
                <GroupsTab
                    isUserInFormedGroup={this.state.courseInformation?.isUserInFormedGroup}
                    groupsFormed={this.state.groups[this.state.currentSection].formed}
                    groupsUnformed={this.state.groups[this.state.currentSection].unformed}
                    isUserInThisSection={
                        this.state.courseInformation?.currentUserSection === this.state.currentSection + 1
                    }
                    onSendRequestModalOpened={this.onSendRequestModalOpened}
                    onUnformedGroupModalOpened={this.onUnformedGroupModalOpened}
                    isLocked={this.state.courseInformation?.isLocked}
                />
            ) : null;
        }

        return {
            title: 'Groups',
            content: (
                <>
                    {this.getDropdownForSections()}
                    {this.state.groups &&
                    (0 <= this.state.currentSection ||
                        this.state.currentSection < this.state.courseInformation.numberOfSections)
                        ? groupsTab
                        : null}
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
                onEditAssignmentModalOpened={this.onEditAssignmentModalOpened}
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
                    curAssignment={dummyCourseAssignments[0]}
                    onClosed={this.onEditAssignmentModalClosed}
                    isOpen={this.state.isEditAssignmentOpen}
                />
                <DeleteAssignmentModal
                    curAssignment={dummyCourseAssignments[0]}
                    onClosed={this.onDeleteAssignmentModalClosed}
                    isOpen={this.state.isDeleteAssignmentOpen}
                />
                <SendRequestModal
                    onClosed={this.onSendRequestModalClosed}
                    isOpen={this.state.isSendRequestModalOpen}
                    onTextChange={this.onTextChanged}
                    text={this.state.currentMessage}
                    isUserAlone={this.state.courseInformation?.isUserAlone}
                    members={this.state.currentMembers}
                    onUserClicked={this.onUserClicked}
                />
                <UnformedGroupModal
                    onClosed={this.onUnformedGroupModalClosed}
                    isOpen={this.state.isUnformedGroupModalOpen}
                    onUserClicked={this.onUserClicked}
                    members={this.state.currentMembers}
                    isFormable={this.state.currentIsFormable}
                    isUserReady={this.state.currentIsUserReady}
                    voteStatus={this.state.currentVoteStatus}
                />
            </>
        );
    };

    render() {
        return (
            <>
                <div class="ui centered grid">
                    <Grid.Row divided>
                        <div class="four wide column">
                            <Segment style={{ boxShadow: 'none', border: '0' }}>{this.getInformationSection()}</Segment>
                        </div>
                        <div class="twelve wide column">
                            {this.props.match.params.assignmentId ? (
                                this.getAssignmentPage()
                            ) : (
                                <Tab tabPanes={this.getCoursePanes()} />
                            )}
                        </div>
                    </Grid.Row>
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
    isUserInFormedGroup: false,
    isUserAlone: false,
    isLocked: true,
    formationDate: '15/24/2020',
    numberOfSections: 3,
    currentUserSection: 1,
};

const dummyCourseAssignments = [
    {
        title: 'Analysis Report',
        caption:
            'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Pariatur optio dolores modi illo, soluta nesciunt? Explicabo dicta ad nulla ea.',
        assignmentId: 1,
        type: 1,
        publisher: 'Elgun Jabrayilzade',
        file: 'file',
        publishmentDate: '12 March 2021 12:00',
        dueDate: new Date(2021, 3, 12, 12, 0),
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

const dummyGroupsLocked = [
    [
        {
            members: [
                {
                    name: '1Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '1Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '1Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 1,
            groupName: 'BilHub',
        },
        {
            members: [
                {
                    name: '2Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '2Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '2Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 2,
            groupName: 'Not Bilhub',
        },
    ],
    [
        {
            members: [
                {
                    name: '1Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '1Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '1Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 1,
            groupName: 'BilHub',
        },
        {
            members: [
                {
                    name: '2Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '2Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '2Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 2,
            groupName: 'Not Bilhub',
        },
    ],
    [
        {
            members: [
                {
                    name: '1Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '1Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '1Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 1,
            groupName: 'BilHub',
        },
        {
            members: [
                {
                    name: '2Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '2Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '2Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 2,
            groupName: 'Not Bilhub',
        },
    ],
];

const dummyGroups = [
    {
        formed: [
            {
                members: [
                    {
                        name: '1Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '1Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '1Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '2Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '2Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '2Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
        unformed: [
            {
                members: [
                    {
                        name: '3Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '3Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '3Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '4Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '4Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '4Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
                isUserInGroup: true,
                voteStatus: '2/3',
                isUserReady: true,
                isFormable: true,
            },
            {
                members: [
                    {
                        name: '4Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '4Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '4Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
                notRequestable: true,
            },
        ],
    },
    {
        formed: [
            {
                members: [
                    {
                        name: '5Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '5Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '5Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '6Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '6Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '6Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
        unformed: [
            {
                members: [
                    {
                        name: '7Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '7Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '7Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '8Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '8Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '8Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
    },
    {
        formed: [
            {
                members: [
                    {
                        name: '9Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '9Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '9Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '10Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '10Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '10Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
        unformed: [
            {
                members: [
                    {
                        name: '11Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '11Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '11Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '12Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '12Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '12Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
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
