import React, { Component } from 'react';
import { Icon, Input, TextArea, Button, Card } from 'semantic-ui-react';

import './Project.css';
import {
    InformationSection,
    MemberElement,
    AssignmentPane,
    GradePane,
    FeedbackPane,
    NewCommentModal,
    EditCommentModal,
    DeleteCommentModal,
} from './ProjectComponents';
import { Tab, FeedbackCardElement, AssignmentCardElement } from '../../commonComponents';
import { ProjectSubmission } from './ProjectSubmission';

export class Project extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: null,
            projectGroup: { members: [] },
            assignments: null,
            grades: null,
            feedbacks: [],

            // States regarding changing left part of the page
            nameEditMode: false,
            informationEditMode: false,
            newName: '',
            newInformation: '',

            // States regarding open models of right part
            isGiveFeedbackOpen: false,
            isEditFeedbackOpen: false,
            isDeleteFeedbackOpen: false,
            isFeedbackSRS: false,
            currentFeedbackText: '',
            currentFeedbackFile: null,
            currentFeedbackGrade: 10,
            currentFeedbackId: 0,

            // States regarding accordion
            accordionActiveIndex: 0,
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

    // Logic Regarding Left Side
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

    // For logic regarding Assignments
    onFeedClicked = (projectId, submissionPageId) => {
        this.props.history.push('/project/' + projectId + '/submission/' + submissionPageId);
    };

    onFeedFileClicked = () => {
        console.log('FILE');
    };

    onFeedPublisherClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    convertFeedsToFeedList = (feeds) => {
        return feeds.map((feed) => {
            const date = 'Publishment Date: ' + feed.publishmentDate + ' / Due Date: ' + feed.dueDate;
            return (
                <AssignmentCardElement
                    title={feed.title}
                    titleClicked={() => this.onFeedClicked(feed.projectId, feed.submissionPageId)}
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

    // For logic regarding grades
    getTableBodyRowsData = (grades) => {
        const persons = grades.persons;

        const personsData = persons.map((person) => {
            return [person.type, person.name, person.grade];
        });

        if (grades.studentsAverage) {
            personsData.push(['Students Average', '', grades.studentsAverage]);
        }

        return personsData;
    };

    getSRSFeedbackContent = (SRSResult) => {
        if (SRSResult) {
            let icons = null;
            if (this.state.projectGroup.isTAorInstructor) {
                icons = (
                    <span>
                        <Icon
                            name="edit"
                            onClick={() =>
                                this.openModelWithComment(
                                    'isEditFeedbackOpen',
                                    true,
                                    SRSResult.commentId,
                                    SRSResult.feedback,
                                    SRSResult.grade
                                )
                            }
                        />
                        <Icon
                            name="delete"
                            onClick={() =>
                                this.openModelWithComment(
                                    'isDeleteFeedbackOpen',
                                    true,
                                    SRSResult.commentId,
                                    SRSResult.feedback,
                                    SRSResult.grade
                                )
                            }
                        />
                    </span>
                );
            }

            return (
                <FeedbackCardElement
                    feedback={SRSResult.feedback}
                    grade={SRSResult.grade}
                    totalGrade={10}
                    author={SRSResult.name}
                    date={SRSResult.date}
                    icons={icons}
                />
            );
        } else if (this.state.projectGroup?.isTAorInstructor) {
            return <Button onClick={() => this.openModal('isGiveFeedbackOpen', true)}>Add SRS Grade</Button>;
        } else {
            return <div>No Feedback</div>;
        }
    };

    getFeedbacksContent = (feedbacks) => {
        if (feedbacks) {
            const feedbackFeedElements = feedbacks.map((feedback) => {
                let icons = null;
                if (this.state.user.userId === feedback.userId) {
                    icons = (
                        <span>
                            <Icon
                                name="edit"
                                onClick={() =>
                                    this.openModelWithComment(
                                        'isEditFeedbackOpen',
                                        false,
                                        feedback.commentId,
                                        feedback.feedback,
                                        feedback.grade
                                    )
                                }
                            />
                            <Icon
                                name="delete"
                                onClick={() =>
                                    this.openModelWithComment(
                                        'isDeleteFeedbackOpen',
                                        false,
                                        feedback.commentId,
                                        feedback.feedback,
                                        feedback.grade
                                    )
                                }
                            />
                        </span>
                    );
                }

                return (
                    <FeedbackCardElement
                        feedback={feedback.feedback}
                        grade={feedback.grade}
                        totalGrade={10}
                        author={feedback.name}
                        date={feedback.date}
                        icons={icons}
                    />
                );
            });

            return <Card.Group>{feedbackFeedElements}</Card.Group>;
        } else {
            return <div>No Feedback</div>;
        }
    };

    getCommentsAsAccordionElements = () => {
        const feedbacks = this.state.feedbacks;

        return [
            { title: 'SRS Feedback', content: this.getSRSFeedbackContent(feedbacks.SRSResult) },
            {
                title: 'Instructor Feedbacks',
                content: this.getFeedbacksContent(feedbacks.InstructorComments),
            },
            { title: 'TA Feedbacks', content: this.getFeedbacksContent(feedbacks.TAComments) },
            { title: 'Student Feedbacks', content: this.getFeedbacksContent(feedbacks.StudentComments) },
        ];
    };

    // Accordion
    handleAccordionClick = (titleProps) => {
        const index = titleProps.index;
        const activeIndex = this.state.accordionActiveIndex;
        const newIndex = activeIndex === index ? -1 : index;

        this.setState({ accordionActiveIndex: newIndex });
    };

    getPaneElements = () => {
        let newCommentButton = null;
        if (this.state.projectGroup.canUserComment) {
            newCommentButton = (
                <Button
                    content="Give Feedback"
                    labelPosition="right"
                    icon="edit"
                    primary
                    onClick={() => this.openModal('isGiveFeedbackOpen', false)}
                />
            );
        }

        return [
            {
                title: 'Assignments',
                content: this.state.assignments ? (
                    <AssignmentPane feedList={this.convertFeedsToFeedList(this.state.assignments)} />
                ) : (
                    <div>No Assignments</div>
                ),
            },
            {
                title: 'Grades',
                content: this.state.grades ? (
                    <GradePane
                        firstBodyRowsData={this.getTableBodyRowsData(this.state.grades)}
                        firstHeaderNames={['User', 'Person', 'Grade']}
                        secondBodyRowsData={[[this.state.grades.projectAverage, this.state.grades.courseAverage]]}
                        secondHeaderNames={[this.state.projectGroup.name + ' Avarage', 'Course Average']}
                        finalGrade={this.state.grades.finalGrade}
                    />
                ) : (
                    <div>No Grades</div>
                ),
            },
            {
                title: 'Feedbacks',
                content: (
                    <FeedbackPane
                        handleClick={(titleProps) => this.handleAccordionClick(titleProps)}
                        activeIndex={this.state.accordionActiveIndex}
                        newCommentButton={newCommentButton}
                        accordionElements={this.getCommentsAsAccordionElements()}></FeedbackPane>
                ),
            },
        ];
    };

    onCurrentFeedbackTextChange = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackText: e.target.value,
        });
    };

    onCurrentFeedbackGradeChange = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackGrade: e.target.value,
        });
    };

    // Models
    openModal = (modelType, isFeedbackSRS) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modelType) {
            this.setState({
                [modelType]: true,
                currentFeedbackGrade: 10,
                currentFeedbackFile: 'empty',
                currentFeedbackText: '',
            });
        }
    };

    openModelWithComment = (modelType, isFeedbackSRS, commentId, commentText, commentGrade, commentFile) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modelType) {
            this.setState({
                currentFeedbackId: commentId,
                [modelType]: true,
                currentFeedbackText: commentText,
                currentFeedbackGrade: commentGrade,
                currentFeedbackFile: commentFile,
            });
        }
    };

    closeModal = (modelType, isSuccess) => {
        this.setState({
            [modelType]: false,
        });
        if (!isSuccess) return;

        if (this.state.isFeedbackSRS) {
            let feed = { ...this.state.feedbacks.SRSResult };
            if (this.state.isEditFeedbackOpen) {
                feed.feedback = this.state.currentFeedbackText;
                feed.grade = this.state.currentFeedbackGrade;
            } else if (this.state.isGiveFeedbackOpen) {
                feed.feedback = this.state.currentFeedbackText;
                feed.grade = this.state.currentFeedbackGrade;
            } else if (this.state.isDeleteFeedbackOpen) {
                feed = null;
            }
            this.setState({
                feedbacks: {
                    ...this.state.feedbacks,
                    SRSResult: feed,
                },
            });
            return;
        }

        if (this.state.isEditFeedbackOpen) {
        } else if (this.state.isGiveFeedbackOpen) {
        } else if (this.state.isDeleteFeedbackOpen) {
        }

        // POST
    };

    // Modals
    getModals = () => {
        console.log(this.state.currentFeedbackText);
        return (
            <>
                <NewCommentModal
                    isOpen={this.state.isGiveFeedbackOpen}
                    closeModal={(isSuccess) => this.closeModal('isGiveFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChange(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChange(e)}
                />
                <EditCommentModal
                    isOpen={this.state.isEditFeedbackOpen}
                    closeModal={(isSuccess) => this.closeModal('isEditFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChange(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChange(e)}
                />
                <DeleteCommentModal
                    isOpen={this.state.isDeleteFeedbackOpen}
                    closeModal={(isSuccess) => this.closeModal('isDeleteFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                />
            </>
        );
    };

    render() {
        const memberElements = this.convertMembersToMemberElement(this.state.projectGroup.members);

        // EDITING GROUP NAME ELEMENTS
        let nameEditIcon = null;
        if (this.state.projectGroup.isNameChangeable && this.state.projectGroup.isInGroup) {
            nameEditIcon = this.state.nameEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeGroupName(this.state.newName);
                        this.toggleEditMode('nameEditMode');
                    }}
                    name={'check'}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
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
        if (this.state.projectGroup.isProjectActive && this.state.projectGroup.isInGroup) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeGroupInformation(this.state.newInformation);
                        this.toggleEditMode('informationEditMode');
                    }}
                    name={'check'}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
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

        const paneElements = this.getPaneElements();
        const modals = this.getModals();

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
                <div className={'FloatingCenterDiv'}>
                    {!this.props.match.params.submissionPageId ? (
                        <Tab panes={paneElements} />
                    ) : (
                        <ProjectSubmission
                            projectName={this.state.projectGroup?.name}
                            projectId={this.props.match.params.projectId}
                            submissionPageId={this.props.match.params.submissionPageId}
                            userId={this.state.user?.userId}
                        />
                    )}
                </div>
                {modals}
            </div>
        );
    }
}

const dummyUser = {
    name: 'Aybala Karakaya',
    userId: 1,
};

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
        publisherId: 1,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        submissionPageId: 1,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'notsubmitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 1,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 2,
        submissionPageId: 2,
        file: 'dummyFile',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 1,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionPageId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 1,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionPageId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 1,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionPageId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 1,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionPageId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publisherId: 1,
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionPageId: 3,
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
        commentId: 1,
        grade: 9.5,
    },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 3,
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
            commentId: 4,
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
            commentId: 5,
            userId: 1,
            userGroupName: 'ClassRoom Helper',
            userGroupId: 5,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
            userGroupName: 'ProjectManager',
            userGroupId: 4,
        },
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
