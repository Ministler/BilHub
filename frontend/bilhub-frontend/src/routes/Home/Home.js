import React, { Component, createRef } from 'react';
import { Icon, Card, Sticky, Rail, Ref, Segment, Grid, GridColumn } from 'semantic-ui-react';

import './Home.css';
import { AssignmentCardElement } from '../../commonComponents';
import { BriefList, TitledIconedBriefElement, TitledDatedBriefElement, ProfilePrompt } from './HomeComponents';

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

    onProjectClicked = (projectId) => {
        this.props.history.push('project/' + projectId);
    };

    convertProjectsToBriefList = (projects) => {
        return projects.map((project) => {
            const icon = project.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
            const title = project.courseCode + '/' + project.projectName;
            return (
                <TitledIconedBriefElement
                    icon={icon}
                    title={title}
                    onClick={() => this.onProjectClicked(project.projectId)}
                />
            );
        });
    };

    onInsturctedCourseClicked = (courseId) => {
        this.props.history.push('course/' + courseId);
    };

    convertInstructedCoursesToBriefList = (instructedCourses) => {
        return instructedCourses.map((course) => {
            const icon = course.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
            const title = course.courseCode;
            return (
                <TitledIconedBriefElement
                    icon={icon}
                    title={title}
                    onClick={() => this.onInsturctedCourseClicked(course.courseId)}
                />
            );
        });
    };

    onFeedClicked = (projectId, projectAssignmentId) => {
        this.props.history.push('project/' + projectId + '/assignment/' + projectAssignmentId);
    };

    onFeedFileClicked = () => {
        console.log('FILE');
    };

    onFeedPublisherClicked = (userId) => {
        this.props.history.push('profile/' + userId);
    };

    convertFeedsToFeedList = (feeds) => {
        return feeds.map((feed) => {
            const date = 'Publishment Date: ' + feed.publishmentDate + ' / Due Date: ' + feed.dueDate;
            return (
                <AssignmentCardElement
                    title={feed.title}
                    titleClicked={() => this.onFeedClicked(feed.projectId, feed.projectAssignmentId)}
                    file={feed.file}
                    fileClicked={this.onFeedFileClicked}
                    status={feed.status}
                    date={date}
                    publisher={feed.publisher}
                    publisherClicked={() => {
                        this.onFeedPublisherClicked(feed.publisherId);
                    }}>
                    {feed.caption}
                </AssignmentCardElement>
            );
        });
    };

    onUpcomingEventClicked = (projectId, projectAssignmentId) => {
        this.props.history.push('project/' + projectId + '/assignment/' + projectAssignmentId);
    };

    convertUpcomingEventsToBriefList = (upcomingAssignments) => {
        return upcomingAssignments.map((assignment) => {
            const title = assignment.courseCode + '/' + assignment.assignmentName;
            return (
                <TitledDatedBriefElement
                    title={title}
                    date={assignment.dueDate}
                    onClick={() => this.onUpcomingEventClicked(assignment.projectId, assignment.projectAssignmentId)}
                />
            );
        });
    };

    onNotGradedAssignmentClicked = (courseId, courseAssignmentId) => {
        this.props.history.push('course/' + courseId + '/assignment/' + courseAssignmentId);
    };

    convertNotGradedAssignmentsToBriefList = (notGradedAssignments) => {
        return notGradedAssignments.map((assignment) => {
            const title = assignment.courseCode + '/' + assignment.assignmentName;
            return (
                <TitledDatedBriefElement
                    title={title}
                    date={assignment.dueDate}
                    onClick={() =>
                        this.onNotGradedAssignmentClicked(assignment.courseId, assignment.courseAssignmentId)
                    }
                />
            );
        });
    };

    onProfilePromptClicked = () => {
        this.props.history.push('profile');
    };
    contextRef = createRef();
    render() {
        let myProjects = this.state.myProjects ? (
            <BriefList title="My Projects">{this.convertProjectsToBriefList(this.state.myProjects)}</BriefList>
        ) : null;

        const instructedCourses = this.state.instructedCourses ? (
            <BriefList title="Insturcted Courses">
                {this.convertInstructedCoursesToBriefList(this.state.instructedCourses)}
            </BriefList>
        ) : null;

        const notGradedAssignments = this.state.notGradedAssignments ? (
            <BriefList title="Not Graded">
                {this.convertNotGradedAssignmentsToBriefList(this.state.notGradedAssignments)}
            </BriefList>
        ) : null;

        return (
            <Grid>
                <GridColumn width={4}>
                    <div className={'HomeDivLeft'}>
                        <ProfilePrompt name={this.state.user?.name} onClick={this.onProfilePromptClicked} />
                        {myProjects}
                        {instructedCourses}
                    </div>
                </GridColumn>
                <GridColumn width={1} />
                <GridColumn width={6}>
                    <Card.Group>{this.convertFeedsToFeedList(this.state.feeds)}</Card.Group>
                </GridColumn>
                <GridColumn width={1} />
                <GridColumn width={4}>
                    <div className={'HomeDivRight'}>
                        <BriefList title="Upcoming">
                            {this.convertUpcomingEventsToBriefList(this.state.upcomingAssignments)}
                        </BriefList>
                        {notGradedAssignments}
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
        projectAssignmentId: 1,
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
        projectAssignmentId: 2,
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
        projectAssignmentId: 3,
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
        projectAssignmentId: 1,
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
        projectAssignmentId: 2,
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
        projectAssignmentId: 3,
    },
];

const dummyUpcomingAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 1,
        projectAssignmentId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 2,
        projectAssignmentId: 2,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        projectId: 3,
        projectAssignmentId: 3,
    },
];

const dummyNotGradedAssignmentsList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        courseId: 1,
        courseAssignmentId: 1,
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
        courseId: 2,
        courseAssignmentId: 2,
    },
];
