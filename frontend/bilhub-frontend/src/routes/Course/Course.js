import React, { Component } from 'react';
import { Segment, TextArea, Icon, Button, Dropdown, Grid, Popup } from 'semantic-ui-react';
import { connect } from 'react-redux';
import axios from 'axios';

import {
    getCourseRequest,
    getSectionRequest,
    getCourseAssignmentRequest,
    getAssignmentFileRequest,
    putCourseRequest,
    getCourseStatisticRequest,
    deleteAssignmentRequest,
    postJoinRequest,
    postMergeRequest,
    postProjectGiveReadyRequest,
    postLeaveGroupRequest,
    postLockCourseRequest,
} from '../../API';
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
import { AllStudentPeerReviewPane } from '../../components/Tab/TabUI';

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

            //Peer Review
            isPeerReviewOpen: false,
            currentPeerReviewSections: [],
            currentPeerReviewGroups: [],
            currentPeerReviewStudents: [],
            currentPeerReviewSection: {},
            currentPeerReviewGroup: {},
            currentPeerReviewStudent: {},
            currentReviews: {},

            //
            courseGrades: {},
            finalGrades: [],
        };
    }

    lockGroup = () => {
        postLockCourseRequest(this.props.match.params.courseId).then((response) => {
            console.log(response.data.data);
        });
    };

    changeCourseInformation = (newInformation) => {
        putCourseRequest(
            this.props.match.params.courseId,
            this.state.courseInformation.name,
            this.state.courseInformation.courseSemester,
            this.state.courseInformation.year,
            this.state.courseInformation.description,
            this.state.courseInformation.lockDate,
            this.state.courseInformation.minGroupSize,
            this.state.courseInformation.maxGroupSize,
            newInformation
        );
        this.setState({courseInformation: newInformation}); 
    };

    onAssignmentFileClicked = (assignmentId) => {
        getAssignmentFileRequest(assignmentId);
    };

    onSendRequestModalClosed = (isSuccess, type) => {
        this.setState({
            isSendRequestModalOpen: false,
            currentMessage: '',
        });
        if (!isSuccess) return;

        console.log(type);
        if (type === 'join') {
            postJoinRequest(this.state.currentGroupId, this.state.currentMessage).then((response) => {
                console.log(response.data.data);
            });
        } else if (type === 'merge') {
            postMergeRequest(this.state.currentGroupId, this.state.currentMessage);
        }
    };

    onUnformedGroupModalClosed = (e, isSuccess, type) => {
        this.setState({
            isUnformedGroupModalOpen: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        console.log(type);
        if (type === 'exit') {
            console.log(this.state.currentGroupId);
            postLeaveGroupRequest(this.state.currentGroupId).then((response) => {
                console.log(response.data.data);
            });
        } else if (type === 'update') {
            postProjectGiveReadyRequest(this.state.currentGroupId, e.target.isReady.checked);
            request = {
                groupId: this.state.currentGroupId,
                isUserReady: e.target.isReady.checked,
            };
        }
    };

    componentDidMount() {
        getCourseRequest(this.props.match.params.courseId).then((response) => {
            if (!response.data.success) return;

            const courseData = response.data?.data;
            console.log(courseData);
            const courseInformation = {
                name: courseData?.name,
                year: courseData?.year,
                courseSemester: courseData?.courseSemester,
                courseName: courseData?.name + '-' + courseData?.year + courseData?.courseSemester,
                description: courseData?.courseInformation,
                instructors: courseData?.instructors,
                formationDate: courseData?.lockDate,
                numberOfSections: courseData?.numberOfSections,
                isCourseActive: courseData?.isActive,
                information: courseData.courseDescription,
                isLocked: courseData.isLocked,
                lockDate: courseData.lockDate,
                currentUserSection: courseData.currentUserSectionId,
                isTAorInstructorOfCourse: courseData.isInstructorOrTAInCourse,
                isUserInFormedGroup: courseData.isUserInFormedGroup,
                isUserAlone: courseData.isUserAlone,
                minGroupSize: courseData.minGroupSize,
                maxGroupSize: courseData.maxGroupSize,
            };

            this.setState({
                courseInformation: courseInformation,
                newInformation: courseInformation.information,
                currentSection: courseInformation.currentUserSection ? courseInformation.currentUserSection - 1 : 0,
            });

            if (!courseData.isLocked) {
                const sectionRequests = [];
                for (let i = 0; i < courseData?.sections.length; i++) {
                    sectionRequests.push(getSectionRequest(courseData?.sections[i].id));
                }

                axios.all(sectionRequests).then(
                    axios.spread((...responses) => {
                        const groups = [];
                        for (let i = 0; i < responses.length; i++) {
                            const data = responses[i].data.data;
                            const section = { formed: [], unformed: [] };
                            for (let group of data.projectGroups) {
                                if (group.confirmationState) {
                                    const members = [];
                                    for (let member of group.groupMembers) {
                                        members.push({
                                            userId: member.id,
                                            name: member.name,
                                        });
                                    }

                                    const group2 = {
                                        groupId: group.id,
                                        members: members,
                                    };
                                    section.formed.push(group2);
                                } else {
                                    const members = [];
                                    let isUserInGroup = false;
                                    let isUserReady = false;
                                    for (let member of group.groupMembers) {
                                        if (member.id == this.props.userId) {
                                            isUserInGroup = true;
                                            isUserReady = group.confirmStateOfCurrentUser;
                                        }
                                        members.push({
                                            userId: member.id,
                                            name: member.name,
                                        });
                                    }

                                    console.log(group);
                                    const group2 = {
                                        groupId: group.id,
                                        members: members,
                                        isUserInGroup: isUserInGroup,
                                        voteStatus: group.confirmedUserNumber + '/' + members.length,
                                        isFormable: this.state.courseInformation?.minGroupSize <= members.length,
                                        notRequestable: this.state.courseInformation?.maxGroupSize <= members.length,
                                        isUserReady: isUserReady,
                                    };
                                    section.unformed.push(group2);
                                }
                            }
                            groups.push(section);
                        }
                        this.setState({
                            groups: groups,
                        });
                    })
                );
            } else {
                const sectionRequests = [];
                for (let i = 0; i < courseData?.sections.length; i++) {
                    sectionRequests.push(getSectionRequest(courseData?.sections[i].id));
                }

                axios.all(sectionRequests).then(
                    axios.spread((...responses) => {
                        console.log(responses);
                        const groups = [];
                        for (let i = 0; i < responses.length; i++) {
                            const data = responses[i].data.data;
                            const section = [];
                            for (let group of data.projectGroups) {
                                const members = [];
                                for (let member of group.groupMembers) {
                                    members.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }
                                const group2 = {
                                    groupName: group.name,
                                    groupId: group.id,
                                    members: members,
                                };
                                section.push(group2);
                            }
                            groups.push(section);
                        }
                        this.setState({
                            groups: groups,
                        });
                    })
                );
            }
        });

        getCourseAssignmentRequest(this.props.match.params.courseId).then((response) => {
            if (!response.data.success) return;

            const data = response.data.data;
            const assignments = [];
            for (let assignment of data) {
                assignments.push(assignment);
            }

            this.setState({
                assignments: assignments,
            });
        });

        getCourseStatisticRequest(this.props.match.params.courseId).then((response) => {
            if (!response.data.success) return;
            let stat = response.data.data;
            const final = [];
            for (let i in stat.ozgurStatDtos) {
                final.push(stat.ozgurStatDtos[i].grades[0]);
            }

            this.setState({ courseGrades: { graders: stat.graders, groups: stat.ozgurStatDtos }, finalGrades: final });
        });
    }

    onNewAssignmentModalClosed = () => {
        this.setState({
            isNewAssignmentOpen: false,
        });
    };
    onEditAssignmentModalClosed = () => {
        this.setState({
            isEditAssignmentOpen: false,
        });
    };

    onDeleteAssignmentModalClosed = () => {
        this.setState({
            isDeleteAssignmentOpen: false,
        });
    };

    onPeerReviewsOpen = (dropdownValues) => {
        this.setState({
            isPeerReviewOpen: true,
        });
    };

    onNewAssignmentModalOpened = () => {
        this.setState({
            isNewAssignmentOpen: true,
        });
    };

    onEditAssignmentModalOpened = (index) => {
        this.setState({
            index: index,
            isEditAssignmentOpen: true,
        });
    };

    onDeleteAssignmentModalOpened = (index) => {
        this.setState({
            index: index,
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
        if (this.state.courseInformation?.isTAorInstructorOfCourse) {
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
        if (this.state.courseInformation?.isTAorInstructorOfCourse) {
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
                    <Grid columns="equal">
                        <Grid.Row>
                            <Grid.Column>{this.getDropdownForSections()}</Grid.Column>
                            {this.state.courseInformation?.isTAorInstructorOfCourse &&
                                !this.state.courseInformation?.isLocked && (
                                    <Grid.Column>
                                        <Button
                                            content="Lock Groups"
                                            labelPosition="right"
                                            icon="lock"
                                            primary
                                            floated="right"
                                            onClick={() => this.lockGroup()}
                                        />
                                    </Grid.Column>
                                )}
                        </Grid.Row>
                    </Grid>
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
            content: <>{getCourseStatistics(this.state.courseGrades, this.state.finalGrades)} </>,
        };
    };

    getNewAssignmentButton = () => {
        let button = null;
        if (this.state.courseInformation?.isTAorInstructorOfCourse) {
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

    downloadFromSection = (value) => {
        if (value === 0) {
            window.alert('You have to enter section.');
        } else {
            const request = {
                course: this.state.courseInformation,
                section: value,
                assignment: -1,
            };
        }
    };

    getDownloadSubmissionsPopup = () => {
        let value = 0;
        let sectionNo = this.state.courseInformation.numberOfSections;
        const sections = [{ key: 0, text: 'All Sections', value: -1 }];
        for (var i = 0; i < sectionNo; i++) {
            sections.push({ key: i + 1, text: 'Section ' + (i + 1), value: i + 1 });
        }
        return (
            <Popup
                flowing
                on="click"
                trigger={
                    <Button color="green" style={{ marginLeft: '10px' }} icon labelPosition="right">
                        Download Submissions <Icon name="download" />
                    </Button>
                }>
                <Dropdown
                    onChange={(e, d) => {
                        value = d.value;
                    }}
                    placeholder="Select Section"
                    item
                    selection
                    options={sections}></Dropdown>
                <Button onClick={() => this.downloadFromSection(value)} style={{ marginLeft: '10px' }} icon>
                    <Icon name="download" />
                </Button>
            </Popup>
        );
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
                        this.onEditAssignmentModalOpened,
                        this.onDeleteAssignmentModalOpened,
                        this.state.courseInformation?.isTAorInstructorOfCourse
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

    handleSectionChange = (data) => {
        let index;
        for (var i = 0; i < data.options.length; i++) {
            if (data.options[i].value === data.value) {
                index = i;
                i = data.options.length;
            }
        }
        this.setState({
            currentPeerReviewSection: data.value,
            currentPeerReviewGroups: this.state.groups[index],
            currentPeerReviewGroup: {},
            currentPeerReviewStudents: [],
            currentPeerReviewStudent: {},
            currentReviews: {},
        });
    };

    handleGroupChange = (data) => {
        let index;
        for (var i = 0; i < data.options.length; i++) {
            if (data.options[i].value === data.value) {
                index = i;
                i = data.options.length;
            }
        }
        this.setState({
            currentPeerReviewGroup: data.value,
            currentPeerReviewStudents: this.state.currentPeerReviewGroups[index].members,
            currentPeerReviewStudent: {},
            currentReviews: {},
        });
    };

    handleStudentChange = (data) => {
        this.setState({
            currentPeerReviewStudent: data.value,
            currentReviews: comingDummyPeerReviews,
        });
    };

    getPeerReviewPane = () => {
        return {
            title: 'Peer Review',
            content: this.state.isPeerReviewOpen ? (
                <AllStudentPeerReviewPane
                    state={{ ...this.state }}
                    handleStudentChange={(data) => this.handleStudentChange(data)}
                    handleGroupChange={(data) => this.handleGroupChange(data)}
                    handleSectionChange={(data) => this.handleSectionChange(data)}
                />
            ) : (
                <Button
                    content="Open Peer Reviews"
                    labelPosition="right"
                    icon="add"
                    primary
                    style={{ marginTop: '20px' }}
                    onClick={this.onPeerReviewsOpen}
                />
            ),
        };
    };

    getCoursePanes = () => {
        return this.state.courseInformation?.isTAorInstructorOfCourse
            ? [this.getGroupsPane(), this.getStatisticsPane(), this.getAssignmentPane(), this.getPeerReviewPane()]
            : [this.getGroupsPane(), this.getStatisticsPane(), this.getAssignmentPane()];
    };
    //,,
    getAssignmentPage = () => {
        return (
            <CourseAssignment
                isTAorInstructorOfCourse={this.state.courseInformation?.isTAorInstructorOfCourse}
                numberOfSections={
                    this.state.courseInformation.numberOfSections ? this.state.courseInformation.numberOfSections : 1
                }
                currentUserSection={this.state.courseInformation?.currentUserSection}
                isCourseActive={this.state.courseInformation?.isCourseActive}
                courseName={this.state.courseInformation?.courseName}
                onEditAssignmentModalOpened={this.onEditAssignmentModalOpened}
                onEditAssignmentModalClosed={this.onEditAssignmentModalClosed}
                onDeleteAssignmentModalOpened={this.onDeleteAssignmentModalOpened}
                onDeleteAssignmentModalClosed={this.onDeleteAssignmentModalClosed}
            />
        );
    };

    deleteAssignment = (assignmentId) => {
        deleteAssignmentRequest(assignmentId);
    };

    getModals = () => {
        return (
            <>
                <NewAssignmentModal
                    courseId={this.props.match.params.courseId}
                    onClosed={this.onNewAssignmentModalClosed}
                    isOpen={this.state.isNewAssignmentOpen}
                />
                {this.state.assignments?.length > this.state.index ? (
                    <EditAssignmentModal
                        curAssignment={this.state.assignments[this.state.index]}
                        onClosed={this.onEditAssignmentModalClosed}
                        isOpen={this.state.isEditAssignmentOpen}
                    />
                ) : null}
                {this.state.assignments?.length > this.state.index ? (
                    <DeleteAssignmentModal
                        curAssignment={
                            this.state.assignments?.length > this.state.index
                                ? this.state.assignments[this.state.index]
                                : null
                        }
                        onClosed={this.onDeleteAssignmentModalClosed}
                        isOpen={this.state.isDeleteAssignmentOpen}
                        deleteAssignment={this.deleteAssignment}
                    />
                ) : null}
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

const goingDummyPeerReviews = [
    {
        ProjectGroupId: 12,
        reviewerId: 1,
        revieweeId: 2,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
    {
        ProjectGroupId: 12,
        reviewerId: 1,
        revieweeId: 4,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
    {
        ProjectGroupId: 12,
        reviewerId: 1,
        revieweeId: 6,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
];

const comingDummyPeerReviews = [
    {
        ProjectGroupId: 12,
        reviewerId: 3,
        revieweeId: 1,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
    {
        ProjectGroupId: 12,
        reviewerId: 2,
        revieweeId: 1,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
    {
        ProjectGroupId: 12,
        reviewerId: 4,
        revieweeId: 1,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
    {
        ProjectGroupId: 12,
        reviewerId: 5,
        revieweeId: 1,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
    {
        ProjectGroupId: 12,
        reviewerId: 6,
        revieweeId: 1,
        Id: 14,
        maxGrade: 5,
        grade: 2,
        comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
        createdAt: new Date(2012, 12, 12, 12, 12),
    },
];
