import React, { Component } from 'react';
import { Grid, GridColumn, Divider, Button } from 'semantic-ui-react';
import { connect } from 'react-redux';

import {
    getAssignmentFeedsRequest,
    getUpcomingAssignmentFeedsRequest,
    getNotGradedAssignmentRequest,
    getAssignmentFileRequest,
    getInstructedCoursesRequest,
    getUserGroupsRequest,
    getCourseRequest,
} from '../../API';
import './Home.css';
import {
    ProfilePrompt,
    convertMyProjectsToBriefList,
    convertInstructedCoursesToBriefList,
    convertUpcomingAssignmentsToBriefList,
    convertNotGradedAssignmentsToBriefList,
    convertAssignmentsToAssignmentList,
} from '../../components';
import reportWebVitals from '../../reportWebVitals';
import axios from 'axios';

import { convertDate, instructerTypeSplit } from '../../utils';

class Home extends Component {
    constructor(props) {
        super(props);
        this.state = {
            myProjects: null,
            instructedCourses: null,
            feeds: null,
            upcomingAssignments: null,
            notGradedAssignments: null,
        };
    }

    componentDidMount() {
        getAssignmentFeedsRequest().then((response) => {
            if (!response.data.success) return;

            const feedData = response.data.data;
            this.setState({
                feeds: feedData,
            });
        });

        getNotGradedAssignmentRequest().then((response) => {
            if (!response.data.success) return;

            const notGradedAssignment = response.data.data;
            this.setState({
                notGradedAssignments: notGradedAssignment,
            });
        });

        getInstructedCoursesRequest(this.props.userId).then((response) => {
            if (!response.data.success) return;

            const instructedCourse = [];
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

        if (this.props.userType !== 'Instructor') {
            getUpcomingAssignmentFeedsRequest().then((response) => {
                if (!response.data.success) return;

                const upcomingData = response.data.data;
                this.setState({
                    upcomingAssignments: upcomingData,
                });
            });

            getUserGroupsRequest(this.props.userId).then((response) => {
                if (!response.data.success) return;

                const data = response.data.data;
                console.log(data);
                const requests = [];
                for (let i = 0; i < data.length; i++) {
                    let courseId = data[i].affiliatedCourseId;
                    requests.push(getCourseRequest(courseId));
                }

                const myProjects = [];
                axios.all(requests).then(
                    axios.spread((...responses) => {
                        for (let i = 0; i < responses.length; i++) {
                            if (!responses[i].data.success) return;

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
        }
    }

    onProfilePromptClicked = () => {
        this.props.history.push('profile');
    };

    onProjectClicked = (projectId) => {
        this.props.history.push('project/' + projectId);
    };

    onCourseClicked = (courseId) => {
        this.props.history.push('course/' + courseId);
    };

    onSubmissionClicked = (projectId, submissionId) => {
        this.props.history.push('project/' + projectId + '/submission/' + submissionId);
    };

    onAssignmentClicked = (courseId, assignmentId) => {
        this.props.history.push('course/' + courseId + '/assignment/' + assignmentId);
    };

    onFeedFileClicked = (assignmentId) => {
        getAssignmentFileRequest(assignmentId);
    };

    test = () => {
        //let date = convertDate(dumyDate);
        //console.log("Input:", dumyDate, "output", date);
        //let {taList, instructerList} = instructerTypeSplit(dumyInstructerList);
        //console.log(taList[0].name, "\n", taList[1].name, "\n", instructerList[0].name, "\n", instructerList[1].name);
    };

    render() {
        const myProjectsComponent = this.state.myProjects
            ? convertMyProjectsToBriefList(this.state.myProjects, this.onProjectClicked, this.onCourseClicked)
            : null;

        const instructedCoursesComponent = this.state.instructedCourses
            ? convertInstructedCoursesToBriefList(this.state.instructedCourses, this.onCourseClicked)
            : null;

        let upcomingAssignmentsComponent = null;
        if (this.props.userType !== 'Instructor') {
            upcomingAssignmentsComponent = convertUpcomingAssignmentsToBriefList(
                this.state.upcomingAssignments,
                this.onSubmissionClicked
            );
        }

        const notGradedAssignmentsComponent = this.state.notGradedAssignments
            ? convertNotGradedAssignmentsToBriefList(this.state.notGradedAssignments, this.onAssignmentClicked)
            : null;

        const feedsComponent = convertAssignmentsToAssignmentList(
            this.state.feeds,
            this.onAssignmentClicked,
            this.onSubmissionClicked,
            this.onFeedFileClicked
        );

        return (
            <Grid>
                <Grid.Row divided>
                    <GridColumn width={4}>
                        <div>
                            <ProfilePrompt name={this.props.userName} onClick={this.onProfilePromptClicked} />
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
                        <Grid>
                            <Grid.Row>
                                <GridColumn width={12}>{feedsComponent}</GridColumn>
                                <GridColumn width={4}>
                                    <div>
                                        {upcomingAssignmentsComponent}
                                        {notGradedAssignmentsComponent}
                                    </div>
                                </GridColumn>
                            </Grid.Row>
                        </Grid>
                    </GridColumn>
                </Grid.Row>
            </Grid>
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

export default connect(mapStateToProps)(Home);

const dumyDate = '2021-05-07T00:00:00';

const dumyInstructerList = [
    {
        email: 'Yusuf@bilkent',
        name: 'Yusuf Uyar',
        userType: 'Student',
    },
    {
        email: 'eray@tuzun',
        name: 'Eray Tuzun',
        userType: 'Instructor',
    },
    {
        email: 'Bilal@bilkent',
        name: 'Bilal Uyar',
        userType: 'Student',
    },
    {
        email: 'Kemal@bilkent',
        name: 'Kemal Uyar',
        userType: 'Instructor',
    },
];

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

const dummyFeedsList = [
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 5,
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 1,
        submissionId: 1,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 5,
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        courseId: 2,
        assignmentId: 2,
        hasFile: 'asd',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        publisherId: 5,
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
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 5,
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 1,
        submissionId: 1,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 5,
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 2,
        submissionId: 2,
        hasFile: 'asd',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        publisherId: 5,
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: new Date(2023, 3, 13, 12, 0),
        dueDate: new Date(2025, 4, 16, 23, 59),
        projectId: 3,
        submissionId: 3,
    },
];

const dummyUpcomingAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: new Date(2020, 3, 16, 23, 59),
        projectId: 1,
        submissionId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: new Date(2020, 3, 16, 23, 59),
        projectId: 2,
        submissionId: 2,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: new Date(2020, 3, 16, 23, 59),
        projectId: 3,
        submissionId: 3,
    },
];

const dummyNotGradedAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: new Date(2020, 3, 16, 23, 59),
        courseId: 1,
        assignmentId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: new Date(2020, 3, 16, 23, 59),
        courseId: 2,
        assignmentId: 2,
    },
];
