import React, { Component } from 'react';
import { Grid, GridColumn, Divider } from 'semantic-ui-react';
import { connect } from 'react-redux';

import './Home.css';
import {
    ProfilePrompt,
    convertMyProjectsToBriefList,
    convertInstructedCoursesToBriefList,
    convertUpcomingAssignmentsToBriefList,
    convertNotGradedAssignmentsToBriefList,
    convertAssignmentsToAssignmentList,
} from '../../components';

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
        this.setState({
            myProjects: dummyMyProjectsList,
            instructedCourses: dummyInstructedCoursesList,
            feeds: dummyFeedsList,
            upcomingAssignments: dummyUpcomingAssignmentsList,
            notGradedAssignments: dummyNotGradedAssignmentsList,
        });
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

    onFeedFileClicked = () => {
        console.log('FEED FILE CLICKED');
    };

    render() {
        const myProjectsComponent = this.state.myProjects
            ? convertMyProjectsToBriefList(this.state.myProjects, this.onProjectClicked)
            : null;

        const instructedCoursesComponent = this.state.instructedCourses
            ? convertInstructedCoursesToBriefList(this.state.instructedCourses, this.onCourseClicked)
            : null;

        let upcomingAssignmentsComponent = null;
        if (this.props.userType !== 'instructor') {
            upcomingAssignmentsComponent = convertUpcomingAssignmentsToBriefList(
                this.state.upcomingAssignments,
                this.onAssignmentClicked
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
            <Grid><Grid.Row divided>
                <GridColumn width={4}>
                    <div>
                        <ProfilePrompt name={this.props.userName} onClick={this.onProfilePromptClicked} />
                        {myProjectsComponent && (
                            <div className="MyProjectsBlock">
                                <h4 style={{ marginLeft: '20px' }}>My Projects</h4>
                                {myProjectsComponent}
                            </div>
                        )}
                        {(myProjectsComponent && instructedCoursesComponent) && (
                            <Divider style={{ width: "70%", margin: "auto", marginTop:"20px"}}/>
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
                    <Grid><Grid.Row>
                        <GridColumn width={12}>{feedsComponent}</GridColumn>
                        <GridColumn width={4}>
                            <div>
                                {upcomingAssignmentsComponent}
                                {notGradedAssignmentsComponent}
                            </div>
                        </GridColumn>
                    </Grid.Row></Grid>
                </GridColumn>
            </Grid.Row></Grid>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        userName: state.name,
        userType: state.userType,
    };
};

export default connect(mapStateToProps)(Home);

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
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 5,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
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
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        courseId: 2,
        assignmentId: 2,
        file: 'asd',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        publisherId: 5,
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
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
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
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
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 2,
        submissionId: 2,
        file: 'asd',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        publisherId: 5,
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionId: 3,
    },
];

const dummyUpcomingAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 1,
        submissionId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 2,
        submissionId: 2,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 3,
        submissionId: 3,
    },
];

const dummyNotGradedAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        courseId: 1,
        assignmentId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        courseId: 2,
        assignmentId: 2,
    },
];
