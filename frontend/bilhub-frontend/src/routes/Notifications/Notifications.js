import React, { Component } from 'react';
import { Grid, GridColumn, Divider, ListItem } from 'semantic-ui-react';
import { connect } from 'react-redux';

import {
    getOutgoingJoinRequest,
    getOutgoingMergeRequest,
    getIncomingMergeRequest,
    getIncomingJoinRequest,
    getUserGroupsRequest,
    getInstructedCoursesRequest,
    getNewCommentsRequest,
    getCourseRequest,
    putJoinRequest,
    putMergeRequest,
} from '../../API';
import './Notifications.css';
import {
    ProfilePrompt,
    convertMyProjectsToBriefList,
    convertInstructedCoursesToBriefList,
    getRequestsAsAccordion,
    Tab,
    getNewFeedbacksAsAccordion,
} from '../../components';
import {
    RequestApprovalModal,
    RequestDisapprovalModal,
    RequestUndoModal,
    RequestDeleteModal,
} from './NotificationsComponents';
import axios from 'axios';
import { NewCommentModal } from '../Project/ProjectComponents';

class Notifications extends Component {
    constructor(props) {
        super(props);
        this.state = {
            myProjects: null,
            instructorCourses: null,

            incomingRequests: null,
            outgoingRequests: null,

            newFeedbacks: null,

            isApprovalModalOpen: false,
            isDisapprovalModalOpen: false,
            isUndoModalOpen: false,
            isDeleteModalOpen: false,

            currentRequestId: null,
            currentRequestType: null,
            currentRequestUserName: null,
        };
    }

    onModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        if (modalType === 'isApprovalModalOpen' || modalType === 'isDisapprovalModalOpen') {
            let isApproved = false;
            if (modalType === 'isApprovalModalOpen') {
                isApproved = true;
            }

            request = {
                requestId: this.state.currentRequestId,
                isApproved: isApproved,
            };
        } else if (modalType === 'isUndoModalOpen') {
            if (this.state.currentRequestType === 'Join') {
                putJoinRequest(this.state.currentRequestId, false);
            } else if (this.state.currentRequestType === 'Merge') {
                putMergeRequest(this.state.currentRequestId, false);
            }
        } else if (modalType === 'isDeleteModalOpen') {
            request = {
                requestId: this.state.currentRequestId,
                deleteRequest: true,
            };
        }
    };

    componentDidMount() {
        let incomingRequests = [];
        incomingRequests.push(getIncomingJoinRequest());
        incomingRequests.push(getIncomingMergeRequest());

        axios.all(incomingRequests).then(
            axios.spread((...responses) => {
                let incomingRequests = { pending: [], unresolved: [], resolved: [] };
                let i = 0;
                for (let response of responses) {
                    let data = response.data.data;
                    for (let req of data) {
                        let request = {};
                        if (req.resolved) {
                            if (i === 0) {
                                let myGroup = [];
                                for (let member of req.requestedGroup.groupMembers) {
                                    myGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                request = {
                                    type: 'Join',
                                    requestId: req.id,
                                    status: 'Dissapproved',
                                    yourGroup: myGroup,
                                    user: {
                                        userId: req.requestingStudent.id,
                                        name: req.requestingStudent.name,
                                    },
                                    course: req.courseName,
                                    approvalDate: req.createdAt,
                                    message: req.description,
                                };
                            } else {
                                let myGroup = [];
                                for (let member of req.receiverGroup.groupMembers) {
                                    myGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                let otherGroup = [];
                                for (let member of req.senderGroup.groupMembers) {
                                    otherGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                request = {
                                    type: 'Merge',
                                    requestId: req.id,
                                    status: 'Dissapproved',
                                    yourGroup: myGroup,
                                    otherGroup: otherGroup,
                                    course: req.courseName,
                                    approvalDate: req.createdAt,
                                    message: req.description,
                                };
                            }
                            incomingRequests.resolved.push(request);
                        } else if (req.accepted) {
                            let myGroup = [];
                            for (let member of req.requestedGroup.groupMembers) {
                                myGroup.push({
                                    userId: member.id,
                                    name: member.name,
                                });
                            }
                            if (i === 0) {
                                request = {
                                    type: 'Join',
                                    requestId: req.id,
                                    status: 'Approved',
                                    yourGroup: myGroup,
                                    user: {
                                        userId: req.requestingStudent.id,
                                        name: req.requestingStudent.name,
                                    },
                                    course: req.courseName,
                                    approvalDate: req.createdAt,
                                    message: req.description,
                                };
                            } else {
                                let myGroup = [];
                                for (let member of req.receiverGroup.groupMembers) {
                                    myGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                let otherGroup = [];
                                for (let member of req.senderGroup.groupMembers) {
                                    otherGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                request = {
                                    type: 'Merge',
                                    requestId: req.id,
                                    status: 'Approved',
                                    yourGroup: myGroup,
                                    otherGroup: otherGroup,
                                    course: req.courseName,
                                    approvalDate: req.createdAt,
                                    message: req.description,
                                };
                            }
                            incomingRequests.resolved.push(request);
                        } else if (req.currentUserVote) {
                            if (i === 0) {
                                let myGroup = [];
                                for (let member of req.requestedGroup.groupMembers) {
                                    myGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                request = {
                                    type: 'Join',
                                    requestId: req.id,
                                    status: req.isAccepted ? 'Approved' : 'Dissapproved',
                                    yourGroup: myGroup,
                                    user: {
                                        userId: req.requestingStudent.id,
                                        name: req.requestingStudent.name,
                                    },
                                    course: req.courseName,
                                    requestDate: req.createdAt,
                                    formationDate: req.lockDate,
                                    message: req.description,
                                    voteStatus: req.votedStudents.split(' ').length,
                                };
                            } else {
                                let myGroup = [];
                                for (let member of req.receiverGroup.groupMembers) {
                                    myGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                let otherGroup = [];
                                for (let member of req.senderGroup.groupMembers) {
                                    otherGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                request = {
                                    type: 'Merge',
                                    requestId: req.id,
                                    status: req.isAccepted ? 'Approved' : 'Dissapproved',
                                    yourGroup: myGroup,
                                    otherGroup: otherGroup,
                                    course: req.courseName,
                                    requestDate: req.createdAt,
                                    formationDate: req.lockDate,
                                    message: req.description,
                                    voteStatus: req.votedStudents.split(' ').length,
                                };
                            }
                            incomingRequests.unresolved.push(request);
                        } else {
                            if (i === 0) {
                                let myGroup = [];
                                for (let member of req.requestedGroup.groupMembers) {
                                    myGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                request = {
                                    type: 'Join',
                                    requestId: req.id,
                                    status: req.isAccepted ? 'Approved' : 'Dissapproved',
                                    yourGroup: myGroup,
                                    user: {
                                        userId: req.requestingStudent.id,
                                        name: req.requestingStudent.name,
                                    },
                                    course: req.courseName,
                                    requestDate: req.createdAt,
                                    formationDate: req.lockDate,
                                    message: req.description,
                                    voteStatus: req.votedStudents.split(' ').length,
                                };
                            } else {
                                let myGroup = [];
                                for (let member of req.receiverGroup.groupMembers) {
                                    myGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                let otherGroup = [];
                                for (let member of req.senderGroup.groupMembers) {
                                    otherGroup.push({
                                        userId: member.id,
                                        name: member.name,
                                    });
                                }

                                request = {
                                    type: 'Merge',
                                    requestId: req.id,
                                    status: req.isAccepted ? 'Approved' : 'Dissapproved',
                                    yourGroup: myGroup,
                                    otherGroup: otherGroup,
                                    course: req.courseName,
                                    requestDate: req.createdAt,
                                    formationDate: req.lockDate,
                                    message: req.description,
                                    voteStatus: req.votedStudents.split(' ').length,
                                };
                            }
                            incomingRequests.pending.push(request);
                        }
                    }
                    i++;
                }

                this.setState({
                    incomingRequests: incomingRequests,
                });
            })
        );

        let outgoingRequests = [];
        outgoingRequests.push(getOutgoingJoinRequest());
        outgoingRequests.push(getOutgoingMergeRequest());

        axios.all(outgoingRequests).then(
            axios.spread((...responses) => {
                let outgoingRequests = { pending: [], unresolved: [], resolved: [] };
                for (let response of responses) {
                    let data = response.data.data;
                }
            })
        );

        getNewCommentsRequest().then((response) => {
            console.log(response);
        });

        getInstructedCoursesRequest(this.props.userId).then((response) => {
            if (!response.data.success) return;

            let instructedCourse = [];
            for (let i = 0; i < response.data.data.length; i++) {
                instructedCourse.push({
                    courseId: response.data.data[i].id,
                    courseCode:
                        response.data.data[i].name +
                        ' ' +
                        response.data.data[i].courseSemester +
                        '-' +
                        response.data.data[i].year,
                    isActive: response.data.data[i].isActive,
                });
            }

            this.setState({
                instructedCourses: instructedCourse,
            });
        });

        getUserGroupsRequest(this.props.userId).then((response) => {
            if (!response.data.success) return;

            let data = response.data.data;
            let requests = [];
            for (let i = 0; i < data.length; i++) {
                let courseId = data[i].affiliatedCourseId;
                requests.push(getCourseRequest(courseId));
            }

            let myProjects = [];
            axios.all(requests).then(
                axios.spread((...responses) => {
                    for (let i = 0; i < responses.length; i++) {
                        if (!responses[i].data.success) {
                            myProjects.push({
                                projectName: data[i].name,
                                projectId: data[i].id,
                            });
                        }

                        let courseData = responses[i].data.data;
                        myProjects.push({
                            courseCode: courseData?.name + '-' + courseData?.year + courseData?.courseSemester,
                            projectName: data[i].name,
                            isActive: courseData.isActive,
                            projectId: data[i].id,
                            courseId: data[i].affiliatedCourse.id,
                            isLocked: data[i].affiliatedCourse.isLocked,
                        });
                        this.setState({
                            myProjects: myProjects,
                        });
                    }
                })
            );
        });

        // this.setState({
        //     outgoingRequests: dummyOutgoingRequests,
        //     newFeedbacks: dummyNewFeedbacks,
        // });
    }

    onProjectClicked = (projectId) => {
        this.props.history.push('/project/' + projectId);
    };

    onCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    onUserClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    onProfilePromptClicked = () => {
        this.props.history.push('/profile');
    };

    onRequestAction = (requestModal, requestId, requestType, userName) => {
        this.setState({
            currentRequestId: requestId,
            currentRequestType: requestType,
            currentRequestUserName: userName,
            [requestModal]: true,
        });
    };

    onSubmissionClicked = (projectId, submissionId) => {
        this.props.history.push('/project/' + projectId + '/submission/' + submissionId);
    };

    getIncomingRequestsPane = () => {
        return {
            title: 'Incoming Requests',
            content: getRequestsAsAccordion(
                this.state.incomingRequests,
                'incoming',
                this.onUserClicked,
                this.onRequestAction
            ),
        };
    };

    getOutgoingRequestsPane = () => {
        return {
            title: 'Outgoing Requests',
            content: getRequestsAsAccordion(
                this.state.outgoingRequests,
                'outgoing',
                this.onUserClicked,
                this.onRequestAction
            ),
        };
    };

    getNewFeedbacksPane = () => {
        return {
            title: 'New Feedbacks',
            content: getNewFeedbacksAsAccordion(
                this.state.newFeedbacks,
                this.onSubmissionClicked,
                this.onProjectClicked
            ),
        };
    };

    getPaneElements = () => {
        return [this.getIncomingRequestsPane(), this.getOutgoingRequestsPane(), this.getNewFeedbacksPane()];
    };

    getModals = () => {
        return (
            <>
                <RequestApprovalModal
                    isOpen={this.state.isApprovalModalOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isApprovalModalOpen', isSuccess)}
                    requestType={this.state.currentRequestType}
                    userName={this.state.currentRequestUserName}
                />
                <RequestDisapprovalModal
                    isOpen={this.state.isDisapprovalModalOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isDisapprovalModalOpen', isSuccess)}
                    requestType={this.state.currentRequestType}
                    userName={this.state.currentRequestUserName}
                />
                <RequestUndoModal
                    isOpen={this.state.isUndoModalOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isUndoModalOpen', isSuccess)}
                    requestType={this.state.currentRequestType}
                    userName={this.state.currentRequestUserName}
                />
                <RequestDeleteModal
                    isOpen={this.state.isDeleteModalOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isDeleteModalOpen', isSuccess)}
                    requestType={this.state.currentRequestType}
                    userName={this.state.currentRequestUserName}
                />
            </>
        );
    };

    render() {
        let myProjectsComponent = this.state.myProjects
            ? convertMyProjectsToBriefList(this.state.myProjects, this.onProjectClicked, this.onCourseClicked)
            : null;
        let instructedCoursesComponent = this.state.instructedCourses
            ? convertInstructedCoursesToBriefList(this.state.instructedCourses, this.onCourseClicked)
            : null;

        return (
            <>
                <Grid>
                    <GridColumn width={4}>
                        <div className={'HomeDivLeft'}>
                            <ProfilePrompt name={this.props.name} onClick={this.onProfilePromptClicked} />
                            {myProjectsComponent && (
                                <div className="MyProjectsBlock">
                                    <h4 style={{ marginLeft: '20px' }}>My Projects</h4>
                                    {myProjectsComponent}
                                </div>
                            )}
                            {myProjectsComponent && instructedCoursesComponent && (
                                <Divider style={{ width: '70%', margin: 'auto', marginTop: '20px' }} />
                            )}
                            {instructedCoursesComponent && (
                                <div className="InstructedCoursesBlock">
                                    <h4 style={{ marginLeft: '20px' }}>Instructed Courses</h4>
                                    {instructedCoursesComponent}
                                </div>
                            )}
                        </div>
                    </GridColumn>
                    <GridColumn width={12}>
                        <Tab tabPanes={this.getPaneElements()} />
                    </GridColumn>
                </Grid>
                {this.getModals()}
            </>
        );
    }
}

let mapStateToProps = (state) => {
    return {
        userType: state.userType,
        name: state.name,
        userId: state.userId,
        token: state.token,
    };
};

export default connect(mapStateToProps)(Notifications);

let dummyIncomingRequests = {
    pending: [
        {
            type: 'Join',
            requestId: 1,
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            user: {
                name: 'Hasan Kaya',
                userId: 1,
            },
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            requestDate: new Date(2021, 3, 12, 0, 15),
            formationDate: new Date(2022, 4, 5, 12, 21),
            voteStatus: '2/5',
        },
        {
            type: 'Merge',
            requestId: 1,
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                    requestOwner: true,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            requestDate: new Date(2021, 3, 12, 0, 15),
            formationDate: new Date(2022, 4, 5, 12, 21),
            voteStatus: '2/5',
        },
    ],
    unresolved: [
        {
            type: 'Join',
            requestId: 1,
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            user: {
                name: 'Hasan Kaya',
                userId: 1,
            },
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            requestDate: new Date(2021, 3, 12, 0, 15),
            formationDate: new Date(2022, 4, 5, 12, 21),
            voteStatus: '2/5',
        },
        {
            type: 'Merge',
            requestId: 1,
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                    requestOwner: true,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            requestDate: new Date(2021, 3, 12, 0, 15),
            formationDate: new Date(2022, 4, 5, 12, 21),
            voteStatus: '2/5',
        },
    ],
    resolved: [
        {
            type: 'Join',
            requestId: 1,
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            status: 'Approved',
            user: {
                name: 'Hasan Kaya',
                userId: 1,
            },
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            approvalDate: new Date(2021, 3, 12, 4, 34),
        },
        {
            type: 'Merge',
            requestId: 1,
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            status: 'Approved',
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                    requestOwner: true,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            approvalDate: new Date(2021, 3, 12, 4, 34),
        },
    ],
};

let dummyOutgoingRequests = {
    pending: [
        {
            type: 'Merge',
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            requestId: 3,
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                    requestOwner: true,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            courseId: 1,
            requestDate: new Date(2021, 3, 12, 12, 34),
            formationDate: new Date(2022, 5, 13, 14, 15),
            voteStatus: '2/5',
        },
    ],
    unresolved: [
        {
            type: 'Join',
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            requestId: 1,
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            courseId: 1,
            requestDate: new Date(2021, 3, 12, 12, 34),
            formationDate: new Date(2022, 5, 13, 14, 15),
            voteStatus: '2/5',
        },
        {
            type: 'Merge',
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            requestId: 1,
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            courseId: 1,
            requestDate: new Date(2021, 3, 12, 12, 34),
            formationDate: new Date(2022, 5, 13, 14, 15),
            voteStatus: '2/5',
        },
    ],
    resolved: [
        {
            type: 'Join',
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            requestId: 1,
            status: 'Approved',
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            courseId: 1,
            approvalDate: new Date(2020, 12, 14, 15, 54),
        },
        {
            type: 'Merge',
            message: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae, optio.',
            requestId: 1,
            status: 'Approved',
            isRequestOwner: false,
            yourGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                    requestOwner: true,
                },
            ],
            otherGroup: [
                {
                    name: 'Hasan Kaya',
                    userId: 1,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            courseId: 1,
            approvalDate: new Date(2020, 12, 14, 15, 54),
        },
    ],
};

let dummyNewFeedbacks = {
    SRSResults: [
        {
            user: {
                name: 'Elgun Jabrayilzade',
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionId: 1,
                projectId: 1,
            },
        },
        {
            user: {
                name: 'Elgun Jabrayilzade',
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
            },
            project: {
                projectName: 'BilHub',
                projectId: 1,
            },
        },
    ],
    InstructorComments: [
        {
            user: {
                name: 'Elgun Jabrayilzade',
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionId: 1,
                projectId: 1,
            },
        },
        {
            user: {
                name: 'Elgun Jabrayilzade',
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
            },
            project: {
                projectName: 'BilHub',
                projectId: 1,
            },
        },
    ],
    TAFeedbacks: [
        {
            user: {
                name: 'Elgun Jabrayilzade',
                userId: 1,
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionId: 1,
                projectId: 1,
            },
        },
        {
            user: {
                name: 'Elgun Jabrayilzade',
                userId: 1,
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            project: {
                projectName: 'BilHub',
                projectId: 1,
            },
        },
    ],
    StudentsFeedbacks: [
        {
            user: {
                name: 'Elgun Jabrayilzade',
                userId: 1,
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionId: 1,
                projectId: 1,
            },
        },
        {
            user: {
                name: 'Elgun Jabrayilzade',
                userId: 1,
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: new Date(2029, 10, 13, 23, 32),
                commentId: 1,
                grade: '10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            project: {
                projectName: 'BilHub',
                projectId: 1,
            },
        },
    ],
};
