import React, { Component } from 'react';
import { Icon, Input, TextArea } from 'semantic-ui-react';

import './Project.css';
import { InformationSection, MemberElement } from './ProjectComponents';

export class Project extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: null,
            projectId: this.props.match.params.id,
            projectGroup: { members: [] },
            assignments: null,
            grades: null,
            feedbacks: null,

            // States Regarding chaning left part of the page
            nameEditMode: false,
            informationEditMode: false,
            newName: '',
            newInformation: '',
        };
    }

    componentDidMount() {
        this.setState({
            user: dummyUser,
            projectGroup: dummyProjectGroup,
            assignments: dummyAssignmentsList,
            grades: dummyGrades,
            feedbacks: dummyFeedbacks,

            newName: dummyProjectGroup.name,
            newInformation: dummyProjectGroup.information,
        });
    }

    onCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    onMemberClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    convertMembersToMemberElement(members) {
        return members.map((member) => {
            return <MemberElement onClick={() => this.onMemberClicked(member.userId)} member={member} />;
        });
    }

    // For two way binding with the input
    onInputChange = (e, stateName) => {
        e.preventDefault();
        this.setState({
            [stateName]: e.target.value,
        });
    };

    changeGroupName = (newName) => {
        // DATAYI BACKENDE GÖNDERİCEZ ŞİMDİLİK BÖYLE
        let projectGroup = { ...this.state.projectGroup };
        projectGroup.name = newName;
        this.setState({
            projectGroup: projectGroup,
        });
    };

    changeGroupInformation = (newInformation) => {
        // DATAYI BACKENDE GÖNDERİCEZ ŞİMDİLİK BÖYLE
        let projectGroup = { ...this.state.projectGroup };
        projectGroup.information = newInformation;
        this.setState({
            projectGroup: projectGroup,
        });
    };

    toggleEditMode = (editMode) => {
        this.setState((prevState) => {
            return {
                [editMode]: !prevState[editMode],
            };
        });
    };

    render() {
        const memberElements = this.convertMembersToMemberElement(this.state.projectGroup.members);

        // EDITING GROUP NAME ELEMENTS
        let nameEditIcon = null;
        if (this.state.projectGroup.isNameChangeable && this.state.user.isInGroup) {
            nameEditIcon = this.state.nameEditMode ? (
                <Icon
                    onClick={() => {
                        this.changeGroupName(this.state.newName);
                        this.toggleEditMode('nameEditMode');
                    }}
                    name={'check'}
                />
            ) : (
                <Icon
                    onClick={() => {
                        this.toggleEditMode('nameEditMode');
                    }}
                    name={'edit'}
                />
            );
        }

        let groupNameElement = this.state.projectGroup.name;
        if (this.state.nameEditMode) {
            groupNameElement = <Input onChange={(e) => this.onInputChange(e, 'newName')} value={this.state.newName} />;
        }

        // EDITING GROUP INFORMATION ELEMENTS
        let informationEditIcon = null;
        if (this.state.projectGroup.isProjectActive && this.state.user.isInGroup) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    onClick={() => {
                        this.changeGroupInformation(this.state.newInformation);
                        this.toggleEditMode('informationEditMode');
                    }}
                    name={'check'}
                />
            ) : (
                <Icon
                    onClick={() => {
                        this.toggleEditMode('informationEditMode');
                    }}
                    name={'edit'}
                />
            );
        }

        let groupInformationElement = this.state.projectGroup.information;
        if (this.state.informationEditMode) {
            groupInformationElement = (
                <TextArea onChange={(e) => this.onInputChange(e, 'newInformation')} value={this.state.newInformation} />
            );
        }

        return (
            <div className={'FloatingPageDiv'}>
                <div className={'FloatingLeftDiv'}>
                    <InformationSection
                        onCourseClicked={() => this.onCourseClicked(this.state.projectGroup.courseId)}
                        courseName={this.state.projectGroup.courseName}
                        groupNameElement={groupNameElement}
                        nameEditIcon={nameEditIcon}
                        memberElements={memberElements}
                        informationElement={groupInformationElement}
                        informationEditIcon={informationEditIcon}
                    />
                </div>
                <div className={'FloatingCenterDiv'}></div>
            </div>
        );
    }
}

const dummyUser = {
    name: 'Aybala Karakaya',
    userType: 'student',
    userId: 1,
    isInGroup: true,
};

const dummyProjectGroup = {
    name: 'BilHub',
    isNameChangeable: true,
    isProjectActive: true,
    courseName: 'CS319-2021Spring',
    courseId: 1,
    members: [
        { name: 'Barış Ogün Yörük', information: 'Frontend', userId: 1 },
        { name: 'Halil Özgür Demir', information: 'Frontend', userId: 2 },
        { name: 'Yusuf Uyar Miraç', information: 'Frontend', userId: 3 },
        { name: 'Aybala Karakaya', information: 'Backend', userId: 4 },
        { name: 'Çağrı Mustafa Durgut', information: 'Backend', userId: 5 },
        { name: 'Oğuzhan Özçelik', information: 'Database', userId: 6 },
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
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 2,
        projectAssignmentId: 2,
        file: 'dummyFile',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        projectAssignmentId: 3,
    },
];

const dummyGrades = {
    persons: [
        {
            name: 'Eray Tüzün',
            type: 'Project Instructor',
            grade: '9.5',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            type: 'Instructor',
            grade: '8.1',
            userId: 2,
        },
        {
            name: 'Erdem Tuna',
            type: 'TA',
            grade: '7.1',
            userId: 3,
        },
        {
            name: 'Elgun Jabrayilzade',
            type: 'TA',
            grade: '8.1',
            userId: 4,
        },
    ],
    insturctorsAverage: 9,
    taAverage: 9.5,
    studentsAverage: 8,
    projectAverage: 7.1,
    courseAverage: 6.5,
    finalGrade: 38,
};

const dummyFeedbacks = {
    SRSResult: {
        name: 'Elgun Jabrayilzade',
        feedback: 'Please download the complete feedback file',
        file: 'dummyFile',
        date: '11 March 2021',
        grade: 9.5,
    },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    TAComments: [
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    StudentComments: [
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};
