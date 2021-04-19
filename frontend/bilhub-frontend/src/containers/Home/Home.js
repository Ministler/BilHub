import React, { Component } from 'react';
import { Icon } from 'semantic-ui-react';

import './Home.css';
import { InfList } from '../../components';
import {
    BriefList,
    TitledIconedBriefElement,
    TitledDatedBriefElement,
    ProfilePrompt,
    FeedElement,
} from './HomeComponents';

export class Home extends Component {
    convertProjectsToBriefList = (projects) => {
        return projects.map((project) => {
            const icon = project.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
            const title = project.courseCode + '/' + project.projectName;
            return <TitledIconedBriefElement icon={icon} title={title} />;
        });
    };

    convertInstructedCoursesToBriefList = (instructedCourses) => {
        return instructedCourses.map((course) => {
            const icon = course.isActive ? <Icon name="lock open" /> : <Icon name="lock" />;
            const title = course.courseCode;
            return <TitledIconedBriefElement icon={icon} title={title} />;
        });
    };

    convertFeedsToFeedList = (feeds) => {
        return feeds.map((feed) => {
            const date = 'Publishment Date: ' + feed.publishmentDate + ' / Due Date: ' + feed.dueDate;
            return (
                <FeedElement title={feed.title} status={feed.status} date={date} publisher={feed.publisher}>
                    {feed.caption}
                </FeedElement>
            );
        });
    };

    convertUpcomingEventsToBriefList = (upcomingEvents) => {
        return upcomingEvents.map((event) => {
            const title = event.courseCode + '/' + event.assignmentName;
            return <TitledDatedBriefElement title={title} date={event.dueDate} />;
        });
    };

    convertNotGradedAssignmentsToBriefList = (notGradedAssignments) => {
        return notGradedAssignments.map((assignment) => {
            const title = assignment.courseCode + '/' + assignment.assignmentName;
            return <TitledDatedBriefElement title={title} date={assignment.dueDate} />;
        });
    };

    render() {
        return (
            <div className={'HomeDiv'}>
                <div className={'HomeDivLeft HomeDivSide'}>
                    <ProfilePrompt name={dummyUser.name} />
                    <BriefList title="My Projects">{this.convertProjectsToBriefList(dummyProjectList)}</BriefList>
                    <BriefList title="Insturcted Courses">
                        {this.convertInstructedCoursesToBriefList(dummyInstructedClassList)}
                    </BriefList>
                </div>
                <div className={'HomeDivMiddle'}>
                    <InfList>{this.convertFeedsToFeedList(dummyFeedsList)}</InfList>
                </div>
                <div className={'HomeDivRight HomeDivSide'}>
                    <BriefList title="My Projects">
                        {this.convertUpcomingEventsToBriefList(dummyUpcomingList)}
                    </BriefList>
                    <BriefList title="Insturcted Courses">
                        {this.convertNotGradedAssignmentsToBriefList(dummyNotGradedAssignmentList)}
                    </BriefList>
                </div>
            </div>
        );
    }
}

const dummyUser = {
    name: 'Aybala Karakaya',
    userType: 'student',
};

const dummyProjectList = [
    {
        courseCode: 'CS319-2021Spring',
        projectName: 'BilHub',
        isActive: true,
    },
    {
        courseCode: 'CS315-2021Spring',
        projectName: 'AGA',
        isActive: true,
    },
    {
        courseCode: 'CS102-2019Fall',
        projectName: 'BilCalendar',
        isActive: false,
    },
];

const dummyInstructedClassList = [
    {
        courseCode: 'CS102-2021Spring',
        isActive: true,
    },
    {
        courseCode: 'CS102-2021Fall',
        isActive: false,
    },
    {
        courseCode: 'CS102-2021Fall',
        isActive: false,
    },
];

const dummyFeedsList = [
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
    },
];

const dummyUpcomingList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
    },
];

const dummyNotGradedAssignmentList = [
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
    },
    {
        courseCode: 'CS102-2021Spring',
        assignmentName: 'Analysis Report',
        dueDate: '16 March 2020, 23:59',
    },
];
