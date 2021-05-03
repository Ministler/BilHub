import React, { Component } from 'react';
import { Icon, Input, TextArea, Segment, Button, Grid, Divider } from 'semantic-ui-react';
import { connect } from 'react-redux';
import axios from 'axios';
import _ from 'lodash';

import './Project.css';
import {
    InformationSection,
    NewCommentModal,
    EditCommentModal,
    DeleteCommentModal,
    NewCommentModal2,
} from './ProjectComponents';
import {
    Tab,
    convertAssignmentsToAssignmentList,
    getGradeTable,
    GradePane,
    getFeedbacksAsAccordion,
    convertMembersToMemberElement,
    FeedbacksPane,
} from '../../components';
import { ProjectSubmission } from './ProjectSubmission';
import { InstructorPeerReviewPane, StudentPeerReviewPane } from '../../components/Tab/TabUI';
import {
    getGroupAssignmentsRequest,
    getGroupInstructorCommentsRequest,
    getGroupStudentCommentsRequest,
    getGroupTACommentsRequest,
    getIsUserInstructorOfGroupRequest,
    getProjectGroupRequest,
} from '../../API/projectGroupAPI/projectGroupGET';
import { putProjectGroupInformationRequest, putUpdateSRSGradeRequest } from '../../API/projectGroupAPI/projectGroupPUT';
import { getAssignmentFileRequest } from '../../API/assignmentAPI/assignmentGET';
import { getProjectGradeByIdRequest } from '../../API/projectGradeAPI/projectGradeGET';
import { inputDateToDateObject } from '../../utils';
import { postProjectGradeRequest } from '../../API/projectGradeAPI/projectGradePOST';
import { deleteProjectGradeRequest } from '../../API/projectGradeAPI/projectGradeDELETE';
import { postPeerGradeRequest } from '../../API/peerGradeAPI/peerGradePOST';
import { deleteSrsGradeRequest } from '../../API/projectGroupAPI/projectGroupDELETE';
import { putProjectGradeRequest } from '../../API/projectGradeAPI/projectGradePUT';
import { getPeerGradeRequestWithReviewee, getPeerGradeRequestWithReviewer } from '../../API/peerGradeAPI/peerGradeGET';
import { getProjectGradeDownloadByIdRequest, putProjectGroupRequest } from '../../API';

class Project extends Component {
    constructor(props) {
        super(props);
        this.state = {
            projectGroup: { members: [] },
            assignments: null,
            grades: null,
            feedbacks: [],

            // States regarding changing left part of the page
            nameEditMode: false,
            informationEditMode: false,
            newName: '',
            newInformation: '',

            // States regarding open modals of right part

            isGiveFeedbackOpen: false,
            isEditFeedbackOpen: false,
            isDeleteFeedbackOpen: false,
            isFeedbackSRS: false,
            currentFeedbackText: '',
            currentFeedbackFile: null,
            currentFeedbackGrade: 10,
            currentMaxFeedbackGrade: 10,
            currentFeedbackId: 0,

            // testing
            currentFeedbackText2: '',
            currentFeedbackGrade2: 10,
            currentMaxFeedbackGrade2: 10,

            //Peer Review
            isPeerReviewOpen: true,
            currentReviewComment: '',
            currentReviewGrade: -1,
            currentPeer: 0,
            maxReviewGrade: 10, //will be taken from project groups peer reviewing assignment
            peerReviews: [],
        };
    }
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    changeGroupName = (newName) => {
        putProjectGroupRequest(this.props.match.params.projectId, this.state.projectGroup.information, newName);
    };

    changeGroupInformation = (newInformation) => {
        putProjectGroupInformationRequest(this.props.match.params.projectId, newInformation);
    };

    onAssignmentFileClicked = (assignmentId) => {
        getAssignmentFileRequest(assignmentId).then((response) => {
            if (!response.data.success) return;
        });
    };

    onModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;

        if (this.state.isFeedbackSRS) {
            if (modalType === 'isGiveFeedbackOpen') {
                putUpdateSRSGradeRequest(this.props.match.params.projectId, this.state.currentFeedbackGrade);
            } else if (modalType === 'isEditFeedbackOpen') {
                putUpdateSRSGradeRequest(this.props.match.params.projectId, this.state.currentFeedbackGrade);
            } else if (modalType === 'isDeleteFeedbackOpen') {
                deleteSrsGradeRequest(this.props.match.params.projectId);
            }
        } else {
            if (modalType === 'isGiveFeedbackOpen') {
                console.log(this.state.currentFeedbackFile);
                postProjectGradeRequest(
                    this.props.match.params.projectId,
                    this.state.currentFeedbackMaxGrade,
                    this.state.currentFeedbackGrade,
                    this.state.currentFeedbackText,
                    this.state.currentFeedbackFile
                );
            } else if (modalType === 'isEditFeedbackOpen') {
                putProjectGradeRequest(
                    this.state.currentFeedbackId,
                    this.state.currentFeedbackMaxGrade,
                    this.state.currentFeedbackGrade,
                    this.state.currentFeedbackText,
                    this.state.currentFeedbackFile
                );
            } else if (modalType === 'isDeleteFeedbackOpen') {
                deleteProjectGradeRequest(this.state.currentFeedbackId);
            }
        }
    };

    componentDidMount() {
        getIsUserInstructorOfGroupRequest(this.props.match.params.projectId, this.props.userId).then((auth) => {
            getProjectGroupRequest(this.props.match.params.projectId).then((response) => {
                if (!response.data.success) return;
                const projectGroupData = response.data.data;
                let isInGroup = false;
                for (let i of projectGroupData.groupMembers) {
                    if (i.id === this.props.userId) {
                        isInGroup = true;
                        break;
                    }
                }
                console.log(projectGroupData.groupMembers);
                const projectInformation = {
                    isInGroup: isInGroup,
                    isTAorInstructor: auth.data.data, //look
                    canUserComment: true,
                    name: projectGroupData.name,
                    isNameChangeable: true,
                    courseName: projectGroupData.affiliatedCourse.name,
                    isProjectActive: true, //look
                    courseId: projectGroupData.affiliatedCourseId,
                    members: projectGroupData.groupMembers,
                    information: projectGroupData.projectInformation,
                    newInformation: projectGroupData.projectInformation,
                    newName: projectGroupData.affiliatedCourse.name,
                };
                if (auth.data.data) {
                    getPeerGradeRequestWithReviewer(this.props.match.params.projectId, this.props.userId).then(
                        (users) => {
                            console.log(users.data.data);
                        }
                    );
                } else {
                    getPeerGradeRequestWithReviewee(
                        parseInt(this.props.match.params.projectId),
                        this.state.currentPeer
                    ).then((users) => {
                        console.log(users.data.data);
                    });
                }
                this.setState({
                    projectGroup: projectInformation,
                });
            });
        });

        //isTAorInstructor

        getGroupAssignmentsRequest(this.props.match.params.projectId).then((response) => {
            if (!response.data.success) return;
            const projectAssignments = response.data.data;
            const temp = [];
            const status = { 0: 'notsubmitted', 1: 'submitted', 2: 'graded' };
            for (var i in projectAssignments) {
                temp.push({
                    title: projectAssignments[i].title,
                    status: status[projectAssignments[i].status],
                    caption: projectAssignments[i].caption,
                    publisher: projectAssignments[i].publisher,
                    dueDate: inputDateToDateObject(projectAssignments[i].dueDate),
                    publishmentDate: inputDateToDateObject(projectAssignments[i].publishmentDate),
                    projectId: this.props.match.params.projectId,
                    submissionId: projectAssignments[i].submissionId,
                });
            }

            this.setState({
                assignments: temp,
            });
        });

        /*const dummyAssignmentsList = [
            {
                title: 'CS319-2021Spring / Desing Report Assignment',
                status: 'graded',
                caption:
                    'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
                publisher: 'Erdem Tuna',
                publishmentDate: new Date(2023, 3, 13, 12, 0),
                dueDate: new Date(2025, 4, 16, 23, 59),
                projectId: 1,
                submissionId: 1,
            },*/

        /* getProjectGradeByIdRequest(this.props.match.params.projectId).then((response) => {
            if (!response.data.success) return;
        });*/

        //this.setState({
        //    assignments: dummyAssignmentsList,
        //    grades: dummyGrades,
        //    feedbacks: dummyFeedbacks,
        //    isPeerReviewOpen: true, due date gecince de kapali
        //});

        const feedbackRequests = [];
        const persons = [];

        feedbackRequests.push(getGroupInstructorCommentsRequest(this.props.match.params.projectId));
        feedbackRequests.push(getGroupTACommentsRequest(this.props.match.params.projectId));
        feedbackRequests.push(getGroupStudentCommentsRequest(this.props.match.params.projectId));
        //feedbackRequests.push(getSubmissionSrsGradeRequest()) graderi nasil alirim dusun
        const feedbacks = { InstructorComments: [], TAComments: [], StudentComments: [] };
        axios.all(feedbackRequests).then(
            axios.spread((...responses) => {
                let instGrade = 0;
                for (let i in responses[0].data.data) {
                    console.log(responses[0].data.data[i]);
                    feedbacks.InstructorComments.push({
                        name: responses[0].data.data[i].userInProjectGradeDto.name,
                        caption: responses[0].data.data[i].comment,
                        grade: responses[0].data.data[i].grade,
                        date: inputDateToDateObject(responses[0].data.data[i].lastEdited),
                        commentId: responses[0].data.data[i].id,
                        userId: responses[0].data.data[i].userInProjectGradeDto.id,
                        hasFile: responses[0].data.data[i].hasFile,
                    });
                    persons.push({
                        name: responses[0].data.data[i].userInProjectGradeDto.name,
                        type: 'Instructor',
                        grade: responses[0].data.data[i].grade,
                        userId: responses[0].data.data[i].userInProjectGradeDto.id,
                    });
                    instGrade += responses[0].data.data[i].grade;
                }
                for (let i in responses[1].data.data) {
                    feedbacks.TAComments.push({
                        name: responses[1].data.data[i].userInProjectGradeDto.name,
                        caption: responses[1].data.data[i].comment,
                        grade: responses[1].data.data[i].grade,
                        date: inputDateToDateObject(responses[1].data.data[i].lastEdited),
                        commentId: responses[1].data.data[i].id,
                        userId: responses[1].data.data[i].userInProjectGradeDto.id,
                        hasFile: responses[1].data.data[i].hasFile,
                    });
                    persons.push({
                        name: responses[1].data.data[i].userInProjectGradeDto.name,
                        type: 'TA',
                        grade: responses[1].data.data[i].grade,
                        userId: responses[1].data.data[i].userInProjectGradeDto.id,
                    });
                    instGrade += responses[1].data.data[i].grade;
                }
                let studentAvg = 0;
                for (let i in responses[2].data.data) {
                    feedbacks.StudentComments.push({
                        name: responses[2].data.data[i].userInProjectGradeDto.name,
                        caption: responses[2].data.data[i].comment,
                        grade: responses[2].data.data[i].grade,
                        date: inputDateToDateObject(responses[2].data.data[i].lastEdited),
                        commentId: responses[2].data.data[i].id,
                        userId: responses[2].data.data[i].userInProjectGradeDto.id,
                        hasFile: responses[2].data.data[i].hasFile,
                    });
                    studentAvg += responses[2].data.data[i].grade;
                }
                studentAvg = studentAvg === 0 ? 0 : studentAvg / responses[2].data.data.length;
                const projectAverage =
                    (studentAvg + instGrade) / (responses[0].data.data.length + responses[1].data.data.length + 1);
                const grades = {
                    persons: persons,
                    studentAvg: studentAvg,
                    projectAverage: projectAverage,
                    courseAverage: 8, //kendim belirledim
                    finalGrade: 40, //kendim belirledim
                };
                this.setState({ feedbacks: feedbacks, grades: grades });
            })
        );

        /*const dummyGrades = {
    persons: [
        {
            name: 'Eray Tüzün',
            type: 'Project Instructor',
            grade: '9.5',
        },
        {
            name: 'Alper Sarıkan',
            type: 'Instructor',
            grade: '8.1',
        },
        {
            name: 'Erdem Tuna',
            type: 'TA',
            grade: '7.1',
        },
        {
            name: 'Elgun Jabrayilzade',
            type: 'TA',
            grade: '8.1',
        },
    ],
    studentsAverage: 8,
    projectAverage: 7.1,
    courseAverage: 6.5,
    finalGrade: 38,
};
*/

        /*const dummyFeedbacks = {
    // SRSResult: {
    //     grade: '9.5',
    //     maxGrade: '11',
    // },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            commentId: 3,
            userId: 'dD3wUcJiDHTM9aDs8livI9HpY3h2',
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            commentId: 3,
            userId: 2,
        },
    ],
    TAComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            commentId: 4,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    StudentComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lcaptionorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            commentId: 5,
            userId: 1,
            userGroupName: 'ClassRoom Helper',
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
            userGroupName: 'ProjectManager',
        },
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};*/
    }

    // LEFT SIDE LOGIC
    onCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
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

    // RIGHT SIDE LOGIC
    onSubmissionClicked = (projectId, submissionId) => {
        this.props.history.push('/project/' + projectId + '/submission/' + submissionId);
    };

    onAuthorClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    // MODAL LOGIC
    onCurrentFeedbackTextChanged = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackText: e.target.value,
        });
    };

    onCurrentFeedbackGradeChanged = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackGrade: e.target.value,
        });
    };

    onCurrentFeedbackMaxGradeChanged = (e) => {
        e.preventDefault();
        this.setState({
            currentMaxFeedbackGrade: e.target.value,
        });
    };

    onCurrentFeedbackTextChanged2 = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackText2: e.target.value,
        });
    };
    onCurrentFeedbackGradeChanged2 = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackGrade2: e.target.value,
        });
    };
    onCurrentFeedbackMaxGradeChanged2 = (e) => {
        e.preventDefault();
        this.setState({
            currentMaxFeedbackGrade2: e.target.value,
        });
    };
    onGiveFeedback = (e) => {
        console.log(this.state.currentFeedbackFile);
        postProjectGradeRequest(
            this.props.match.params.projectId,
            this.state.currentMaxFeedbackGrade2,
            this.state.currentFeedbackGrade2,
            this.state.currentFeedbackText2,
            this.state.currentFeedbackFile
        );
    };

    onModalOpened = (modalType, isFeedbackSRS) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modalType) {
            this.setState({
                [modalType]: true,
                currentFeedbackGrade: 10,
                currentMaxFeedbackGrade: 10,
                currentFeedbackFile: 'empty',
                currentFeedbackText: '',
            });
        }
    };

    onModalOpenedWithComment = (
        modalType,
        isFeedbackSRS,
        commentId,
        commentText,
        commentGrade,
        commentFile,
        SRSMaxGrade
    ) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
                currentMaxFeedbackGrade: SRSMaxGrade,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
                currentMaxFeedbackGrade: 10,
            });
        }

        if (modalType) {
            this.setState({
                currentFeedbackId: commentId,
                [modalType]: true,
                currentFeedbackText: commentText,
                currentFeedbackGrade: commentGrade,
                currentFeedbackFile: commentFile,
            });
        }
    };

    getInformationPart = () => {
        return (
            <InformationSection
                onCourseClicked={() => this.onCourseClicked(this.state.projectGroup.courseId)}
                courseName={this.state.projectGroup.courseName}
                groupNameElement={this.getGroupNameElement()}
                nameEditIcon={this.getNameEditIcon()}
                memberElements={convertMembersToMemberElement(this.state.projectGroup.members, this.onUserClicked)}
                informationElement={this.getGroupInformationItem()}
                informationEditIcon={this.getInformationEditIcon()}
            />
        );
    };

    getGroupNameElement = () => {
        let groupNameElement = this.state.projectGroup.name;
        if (this.state.nameEditMode) {
            groupNameElement = (
                <Input
                    className="GroupNameChangeInput"
                    onChange={(e) => this.onInputChanged(e, 'newName')}
                    value={this.state.newName}
                />
            );
        }

        return groupNameElement;
    };

    getNameEditIcon = () => {
        // EDITING GROUP NAME ELEMENTS
        let nameEditIcon = null;
        if (this.state.projectGroup.isNameChangeable && this.state.projectGroup.isInGroup) {
            nameEditIcon = this.state.nameEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeGroupName(this.state.newName);
                        this.onEditModeToggled('nameEditMode');
                    }}
                    name={'check'}
                    color="blue"
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('nameEditMode');
                    }}
                    name={'edit'}
                    color="blue"
                />
            );
        }
        return nameEditIcon;
    };

    getGroupInformationItem = () => {
        let groupInformationElement = this.state.projectGroup.information;
        if (this.state.informationEditMode) {
            groupInformationElement = (
                <TextArea
                    onChange={(e) => this.onInputChanged(e, 'newInformation')}
                    value={this.state.newInformation}
                />
            );
        }
        return groupInformationElement;
    };

    getInformationEditIcon = () => {
        // EDITING GROUP INFORMATION ELEMENTS
        let informationEditIcon = null;
        if (this.state.projectGroup.isProjectActive && this.state.projectGroup.isInGroup) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeGroupInformation(this.state.newInformation);
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

    getAssignmentPane = () => {
        return {
            title: 'Assignments',
            content: this.state.assignments ? (
                convertAssignmentsToAssignmentList(
                    this.state.assignments,
                    null,
                    this.onSubmissionClicked,
                    this.onAssignmentFileClicked
                )
            ) : (
                <div>No Assignments</div>
            ),
        };
    };

    getGradesPane = () => {
        return {
            title: 'Grades',
            content: this.state.grades ? (
                <GradePane
                    tables={getGradeTable(
                        this.state.grades,
                        this.state.grades?.projectAverage,
                        this.state.grades?.courseAverage,
                        this.state.projectGroup?.name
                    )}
                    finalGrade={this.state.grades.finalGrade}
                />
            ) : (
                <div>No Grades</div>
            ),
        };
    };

    getNewCommentButton = () => {
        let newCommentButton = null;
        if (this.state.projectGroup?.canUserComment && this.state.projectGroup.isInGroup) {
            newCommentButton = (
                <Button
                    content="Give Feedback"
                    labelPosition="right"
                    icon="plus"
                    primary
                    onClick={() => this.onModalOpened('isGiveFeedbackOpen', false)}
                />
            );
        }

        return newCommentButton;
    };

    onFeedbackFileClicked = (feedbackId) => {
        getProjectGradeDownloadByIdRequest(feedbackId);
    };

    onFileChanged = (file) => {
        console.log(file);
        this.setState({
            currentFeedbackFile: file,
        });
    };

    getFeedbacksPane = () => {
        const newCommentButton = this.getNewCommentButton();
        const content = (
            <Grid>
                {/* <div class="sixteen wide column">
                    <p>Your Feedback:</p>
                    <Form reply style={{width: "95%"}}>
                        <Form.TextArea rows="5"/>
                        <Button content='Upload File' floated='left' Compact icon labelPosition='right'>Upload File<Icon name='file' /></Button>
                        <Button content='Grade Placeholder' floated='left'/>
                        <Button content='Give Feedback' primary floated='right' Compact/>
                    </Form>
                </div>
                <div class="sixteen wide column" style={{marginTop: "-20px"}}>
                    <Divider/>
                </div> */}
                <div class="sixteen wide column">
                    <NewCommentModal2
                        text={this.state.currentFeedbackText2}
                        grade={this.state.currentFeedbackGrade2}
                        maxGrade={this.state.currentMaxFeedbackGrade2}
                        onTextChange={(e) => this.onCurrentFeedbackTextChanged2(e)}
                        onGradeChange={(e) => this.onCurrentFeedbackGradeChanged2(e)}
                        onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged2(e)}
                        onGiveFeedback={(e) => this.onGiveFeedback(e)}
                        onFileChanged={this.onFileChanged}
                    />
                </div>
                <div class="sixteen wide column" style={{ marginTop: '-20px' }}>
                    <Divider />
                </div>
                <div class="sixteen wide column" style={{ marginTop: '-20px' }}>
                    <FeedbacksPane
                        feedbacksAccordion={getFeedbacksAsAccordion(
                            this.state.feedbacks,
                            this.state.projectGroup?.isTAorInstructor,
                            this.onModalOpenedWithComment,
                            this.onAuthorClicked,
                            this.props.userId,
                            this.onFeedbackFileClicked,
                            this.onModalOpened
                        )}
                        //newCommentButton={newCommentButton}
                    />
                </div>
            </Grid>
        );
        return {
            title: 'Feedbacks',
            content: content,
        };
    };

    changeReviewingPeer = (userId) => {
        let currentReview = {};
        for (var i = 0; i < this.state.peerReviews.length; i++) {
            if (this.state.peerReviews[i].revieweeId === userId) {
                currentReview = { ...this.state.peerReviews[i] };
                i = this.state.peerReviews.length;
            }
        }
        console.log(currentReview);

        this.setState({
            currentPeer: userId,
            currentReviewComment: currentReview.comment ? currentReview.comment : '',
            currentReviewGrade: currentReview.grade ? currentReview.grade : -1,
        });
    };

    changeViewingPeer = (userId) => {
        this.setState({ currentPeer: userId });
    };

    submitReview = () => {
        console.log(this.state.currentReviewGrade);
        if (this.state.currentReviewGrade === -1 || this.state.currentPeer === 0 /* check existance */) {
            window.alert('You need to enter both grade and reviewee');
        } /*if()*/ else {
            postPeerGradeRequest(
                this.props.match.params.projectId,
                this.state.currentPeer,
                this.state.currentReviewGrade,
                this.state.currentReviewComment
            );
        }
    };

    getPeerReviewPane = () => {
        return {
            title: 'Peer Review',
            content: this.state.isPeerReviewOpen ? (
                this.props.userType === 'student' ? (
                    <StudentPeerReviewPane
                        curUser={{ name: this.props.userName, userId: this.props.userId }} //dummy
                        group={this.state.projectGroup}
                        changePeer={(userId) => this.changeReviewingPeer(userId)}
                        currentPeer={this.state.currentPeer}
                        commentChange={(d) => this.setState({ currentReviewComment: d.value })}
                        gradeChange={(d) => this.setState({ currentReviewGrade: d.value })}
                        comment={this.state.currentReviewComment}
                        grade={this.state.currentReviewGrade}
                        submitReview={this.submitReview}
                        maxGrade={this.state.maxReviewGrade}
                    />
                ) : (
                    <InstructorPeerReviewPane
                        peerReviews={this.state.peerReviews} //This will change according to currentPeer
                        group={this.state.projectGroup}
                        userId={this.props.userId}
                        changePeer={(userId) => this.changeViewingPeer(userId)}
                        currentPeer={this.state.currentPeer}
                    />
                )
            ) : null,
        };
    };

    getPaneElements = () => {
        if (this.state.isPeerReviewOpen)
            return [this.getAssignmentPane(), this.getGradesPane(), this.getFeedbacksPane(), this.getPeerReviewPane()];
        else return [this.getAssignmentPane(), this.getGradesPane(), this.getFeedbacksPane()];
    };

    // Modals
    getModals = () => {
        return (
            <>
                <NewCommentModal
                    isOpen={this.state.isGiveFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isGiveFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                    onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged(e)}
                />
                <EditCommentModal
                    isOpen={this.state.isEditFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isEditFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                    onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged(e)}
                />
                <DeleteCommentModal
                    isOpen={this.state.isDeleteFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isDeleteFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                />
            </>
        );
    };

    render() {
        return (
            <div class="ui centered grid">
                <Grid.Row divided>
                    <div class="four wide column">
                        <Segment style={{ boxShadow: 'none', border: '0' }}>{this.getInformationPart()}</Segment>
                    </div>
                    <div class="twelve wide column">
                        {!this.props.match.params.submissionId ? (
                            <Tab tabPanes={this.getPaneElements()} />
                        ) : (
                            <ProjectSubmission
                                projectName={this.state.projectGroup?.name}
                                projectId={this.props.match.params.projectId}
                                submissionId={this.props.match.params.submissionId}
                                userId={this.props.userId}
                            />
                        )}
                    </div>
                </Grid.Row>
                {this.getModals()}
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        userName: state.name,
        userType: state.userType,
        userId: state.userId,
    };
};

export default connect(mapStateToProps)(Project);

const dummyProjectGroup = {
    isInGroup: true,
    isTAorInstructor: true,
    canUserComment: true,
    name: 'BilHub',
    isNameChangeable: true,
    isProjectActive: true,
    courseName: 'CS319-2021Spring',
    courseId: 1,
    members: [
        { name: 'Barış Ogün Yörük', userId: 1 },
        { name: 'Halil Özgür Demir', userId: 2 },
        { name: 'Yusuf Uyar Miraç', userId: 3 },
        { name: 'Aybala Karakaya', userId: 4 },
        { name: 'Çağrı Mustafa Durgut', userId: 5 },
        { name: 'Oğuzhan Özçelik', userId: 6 },
    ],
    information:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Deleniti reiciendis quae provident, fugit animi in tempore laudantium doloribus ipsum repellat voluptates ullam corporis dignissimos, porro sit optio culpa laboriosam explicabo consequuntur cum adipisci rerum quibusdam quas debitis. Nemo error accusantium tempora. Nisi autem, ipsum laboriosam aperiam quam harum debitis doloremque.',
};

const dummyAssignmentsList = [
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 1,
        submissionId: 1,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'notsubmitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 2,
        submissionId: 2,
        file: 'dummyFile',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 3,
        submissionId: 3,
    },
];

const dummyGrades = {
    persons: [
        {
            name: 'Eray Tüzün',
            type: 'Project Instructor',
            grade: '9.5',
        },
        {
            name: 'Alper Sarıkan',
            type: 'Instructor',
            grade: '8.1',
        },
        {
            name: 'Erdem Tuna',
            type: 'TA',
            grade: '7.1',
        },
        {
            name: 'Elgun Jabrayilzade',
            type: 'TA',
            grade: '8.1',
        },
    ],
    studentsAverage: 8,
    projectAverage: 7.1,
    courseAverage: 6.5,
    finalGrade: 38,
};

const dummyPeerReview = {
    ProjectGroupId: 12,
    reviewerId: 1,
    revieweeId: 2,
    Id: 14,
    maxGrade: 5,
    grade: 2,
    comment: 'lorem5 ur adipisicing elit. Cumque neque ullam a 0',
    createdAt: new Date(2012, 12, 12, 12, 12),
};

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

const dummyFeedbacks = {
    // SRSResult: {
    //     grade: '9.5',
    //     maxGrade: '11',
    // },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            commentId: 3,
            userId: 'dD3wUcJiDHTM9aDs8livI9HpY3h2',
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            commentId: 3,
            userId: 2,
        },
    ],
    TAComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            commentId: 4,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    StudentComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lcaptionorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            commentId: 5,
            userId: 1,
            userGroupName: 'ClassRoom Helper',
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
            userGroupName: 'ProjectManager',
        },
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 4, 11),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 4, 11),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};
