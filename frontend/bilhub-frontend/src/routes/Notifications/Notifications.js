import React, { Component } from 'react';
import { Grid, GridColumn } from 'semantic-ui-react';
import { connect } from 'react-redux';

import './Notifications.css';
import {
    ProfilePrompt,
    convertMyProjectsToBriefList,
    convertInstructedCoursesToBriefList,
    convertRequestsToRequestsList,
} from '../../components';

class Notifications extends Component {
    constructor(props) {
        super(props);
        this.state = {
            myProjects: null,
            instructorCourses: null,

            incomingRequests: null,
            outgoingRequests: null,

            newFeedbacks: null,
        };
    }

    componentDidMount() {
        this.setState({
            myProjects: dummyMyProjectsList,
            instructedCourses: dummyInstructedCoursesList,

            incomingRequests: dummyIncomingRequests,
            outgoingRequests: dummyOutgoingRequests,
            newFeedbacks: dummyNewFeedbacks,
        });
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

    onRequestApproved = (requestId) => {};

    onRequestDisapproved = (requestId) => {};

    onSubmissionClicked = (projectId, submissionId) => {
        this.props.history.push('/project/' + projectId + '/submission/' + submissionId);
    };

    render() {
        const myProjectsComponent = this.state.myProjects
            ? convertMyProjectsToBriefList(this.state.myProjects, this.onProjectClicked)
            : null;
        const instructedCoursesComponent = this.state.instructedCourses
            ? convertInstructedCoursesToBriefList(this.state.instructedCourses, this.onCourseClicked)
            : null;

        return (
            <Grid>
                <GridColumn width={4}>
                    <div className={'HomeDivLeft'}>
                        <ProfilePrompt name={this.props.name} onClick={this.onProfilePromptClicked} />
                        {myProjectsComponent}
                        {instructedCoursesComponent}
                    </div>
                </GridColumn>
                <GridColumn width={12}>
                    <div>
                        {convertRequestsToRequestsList(
                            this.state.incomingRequests?.pending,
                            'pending',
                            this.onUserClicked,
                            this.onCourseClicked,
                            this.onRequestApproved,
                            this.onRequestDisapproved
                        )}
                    </div>
                </GridColumn>
            </Grid>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        userType: state.userType,
        name: state.name,
        userId: state.userId,
        token: state.token,
    };
};

export default connect(mapStateToProps)(Notifications);

const dummyMyProjectsList = [
    {
        courseCode: 'CS319-2021Spring',
        projectName: 'BilHub',
        isActive: true,
        projectId: 1,
    },
    {
        courseCode: 'CS315-2021Spring',
        projectName: 'AGA',
        isActive: true,
        projectId: 2,
    },
    {
        courseCode: 'CS102-2019Fall',
        projectName: 'BilCalendar',
        isActive: false,
        projectId: 3,
    },
];

const dummyInstructedCoursesList = [
    {
        courseCode: 'CS102-2021Spring',
        isActive: true,
        courseId: 1,
    },
    {
        courseCode: 'CS102-2021Fall',
        isActive: false,
        courseId: 2,
    },
    {
        courseCode: 'CS102-2021Fall',
        isActive: false,
        courseId: 3,
    },
];

const dummyIncomingRequests = {
    pending: [
        {
            type: 'Join',
            requestId: 1,
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
            courseId: 1,
            requestDate: '12 March 2021',
            formationDate: '22 March 2021',
            numberOfApprovals: 2,
            requiredNumberOfApprovals: 5,
        },
        {
            type: 'Merge',
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
                    requestOwner: true,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            courseId: 1,
            requestDate: '12 March 2021',
            formationDate: '22 March 2021',
            numberOfApprovals: 2,
            requiredNumberOfApprovals: 5,
        },
    ],
    unresolved: [
        {
            type: 'Join',
            requestId: 1,
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
            courseId: 1,
            requestDate: '12 March 2021',
            formationDate: '22 March 2021',
            numberOfApprovals: 2,
            requiredNumberOfApprovals: 5,
        },
        {
            type: 'Merge',
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
                    requestOwner: true,
                },
                {
                    name: 'Ayşe Kaya',
                    userId: 2,
                },
            ],
            course: 'CS315-Spring2020',
            courseId: 1,
            requestDate: '12 March 2021',
            formationDate: '22 March 2021',
            numberOfApprovals: 2,
            requiredNumberOfApprovals: 5,
        },
    ],
    resolved: [
        {
            type: 'Join',
            requestId: 1,
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
            courseId: 1,
            approvalDate: '12 March 2021',
        },
        {
            type: 'Merge',
            requestId: 1,
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
            courseId: 1,
            approvalDate: '12 March 2021',
        },
        {
            type: 'Join',
            requestId: 1,
            status: 'Disapproved',
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
            courseId: 1,
            approvalDate: '12 March 2021',
        },
        {
            type: 'Merge',
            requestId: 1,
            status: 'Disapproved',
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
            courseId: 1,
            approvalDate: '12 March 2021',
        },
    ],
};

const dummyOutgoingRequests = {
    pending: [
        {
            type: 'Merge',
            requestId: 1,
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
            requestDate: '12 March 2021',
            formationDate: '22 March 2021',
            numberOfApprovals: 2,
            requiredNumberOfApprovals: 5,
        },
    ],
    unresolved: [
        {
            type: 'Join',
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
            requestDate: '12 March 2021',
            formationDate: '22 March 2021',
            numberOfApprovals: 2,
            requiredNumberOfApprovals: 5,
        },
        {
            type: 'Merge',
            requestId: 1,
            isRequestOwner: true,
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
            requestDate: '12 March 2021',
            formationDate: '22 March 2021',
            numberOfApprovals: 2,
            requiredNumberOfApprovals: 5,
        },
    ],
    resolved: [
        {
            type: 'Join',
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
            approvalDate: '12 March 2021',
        },
        {
            type: 'Merge',
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
                    isRequestOwner: true,
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
            approvalDate: '12 March 2021',
        },
        {
            type: 'Join',
            requestId: 1,
            status: 'Disapproved',
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
            approvalDate: '12 March 2021',
        },
        {
            type: 'Merge',
            requestId: 1,
            status: 'Disapproved',
            isRequestOwner: true,
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
            courseId: 1,
            approvalDate: '12 March 2021',
        },
    ],
};

const dummyNewFeedbacks = {
    SRSResults: [
        {
            user: {
                name: 'Elgun Jabrayilzade',
                userId: 1,
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionPageId: 1,
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
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
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
    InstructorComments: [
        {
            user: {
                name: 'Elgun Jabrayilzade',
                userId: 1,
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionPageId: 1,
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
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
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
    TAFeedbacks: [
        {
            user: {
                name: 'Elgun Jabrayilzade',
                userId: 1,
            },
            feedback: {
                caption: 'Please download the complete feedback file',
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionPageId: 1,
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
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
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
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
            },
            course: {
                courseName: 'CS319-2021Spring',
                courseId: 1,
            },
            submission: {
                assignmentName: 'Analiz Report',
                submissionPageId: 1,
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
                date: '11 March 2021',
                commentId: 1,
                grade: '10/10',
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
