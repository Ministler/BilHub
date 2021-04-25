import React, { Component } from 'react';
import { Grid, GridColumn } from 'semantic-ui-react';

import './Home.css';
import {
    ProfilePrompt,
    convertMyProjectsToBriefList,
    convertInstructedCoursesToBriefList,
    convertUpcomingAssignmentsToBriefList,
    convertNotGradedAssignmentsToBriefList,
    convertAssignmentsToAssignmentList,
} from '../../commonComponents';

export class Home extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: null,
            myProjects: null,
            instructedCourses: null,
            feeds: [],
            upcomingAssignments: [],
            notGradedAssignments: null,
        };
    }

    componentDidMount() {
        this.setState({
            user: dummyUser,
            myProjects: dummyMyProjectsList,
            instructedCourses: dummyInstructedCoursesList,
            feeds: dummyFeedsList,
            upcomingAssignments: dummyUpcomingAssignmentsList,
            notGradedAssignments: dummyNotGradedAssignmentsList,
        });
    }

    // ON CLICK LISTENERS
    onProfilePromptClicked = () => {
        this.props.history.push('profile');
    };

    onProjectClicked = (projectId) => {
        this.props.history.push('project/' + projectId);
    };
    onInsturctedCourseClicked = (courseId) => {
        this.props.history.push('course/' + courseId);
    };

    onUpcomingAssignmentClicked = (projectId, submissionPageId) => {
        this.props.history.push('project/' + projectId + '/submission/' + submissionPageId);
    };

    onNotGradedAssignmentClicked = (courseId, assignmentPageId) => {
        this.props.history.push('course/' + courseId + '/assignment/' + assignmentPageId);
    };

    onFeedClicked = (projectId, submissionPageId) => {
        this.props.history.push('project/' + projectId + '/submission/' + submissionPageId);
    };

    onFeedFileClicked = () => {
        console.log('FEED FILE CLICKED');
    };

    render() {
        const myProjectsComponent = this.state.myProjects
            ? convertMyProjectsToBriefList(this.state.myProjects, this.onProjectClicked)
            : null;

        const instructedCoursesComponent = this.state.instructedCourses
            ? convertInstructedCoursesToBriefList(this.state.instructedCourses, this.onInsturctedCourseClicked)
            : null;

        const upcomingAssignmentsComponent = convertUpcomingAssignmentsToBriefList(
            this.state.upcomingAssignments,
            this.onUpcomingAssignmentClicked
        );

        const notGradedAssignmentsComponent = this.state.notGradedAssignments
            ? convertNotGradedAssignmentsToBriefList(this.state.notGradedAssignments, this.onNotGradedAssignmentClicked)
            : null;

        const feedsComponent = this.state.feeds ? (
            convertAssignmentsToAssignmentList(this.state.feeds, this.onFeedClicked, this.onFeedFileClicked)
        ) : (
            <div>You Dont Have Any New Feed</div>
        );

        return (
            <Grid>
                <GridColumn width={4}>
                    <div className={'HomeDivLeft'}>
                        <ProfilePrompt name={this.state.user?.name} onClick={this.onProfilePromptClicked} />
                        {myProjectsComponent}
                        {instructedCoursesComponent}
                    </div>
                </GridColumn>
                <GridColumn width={1} />
                <GridColumn width={6}>{feedsComponent}</GridColumn>
                <GridColumn width={1} />
                <GridColumn width={4}>
                    <div className={'HomeDivRight'}>
                        {upcomingAssignmentsComponent}
                        {notGradedAssignmentsComponent}
                    </div>
                </GridColumn>
            </Grid>
        );
    }
}

const dummyUser = {
    name: 'Aybala Karakaya',
    userId: 1,
};

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
        submissionPageId: 1,
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
        submissionPageId: 2,
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
        submissionPageId: 3,
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
        submissionPageId: 1,
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
        submissionPageId: 2,
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
        submissionPageId: 3,
    },
];

const dummyUpcomingAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 1,
        submissionPageId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 2,
        submissionPageId: 2,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 3,
        submissionPageId: 3,
    },
];

const dummyNotGradedAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        courseId: 1,
        assignmentPageId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        courseId: 2,
        assignmentPageId: 2,
    },
];
