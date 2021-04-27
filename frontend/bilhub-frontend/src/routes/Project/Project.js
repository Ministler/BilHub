import React, { Component } from 'react';
import { Icon, Input, TextArea, Segment, Button } from 'semantic-ui-react';

import './Project.css';
import { InformationSection, NewCommentModal, EditCommentModal, DeleteCommentModal } from './ProjectComponents';
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

    // LEFT SIDE LOGIC
    onCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    onMemberClicked = (userId) => {
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

    changeGroupName = (newName) => {
        let projectGroup = { ...this.state.projectGroup };
        projectGroup.name = newName;
        this.setState({
            projectGroup: projectGroup,
        });
    };

    changeGroupInformation = (newInformation) => {
        let projectGroup = { ...this.state.projectGroup };
        projectGroup.information = newInformation;
        this.setState({
            projectGroup: projectGroup,
        });
    };

    // RIGHT SIDE LOGIC
    onAssignmentClicked = (submissionPageId) => {
        this.props.history.push('/project/' + this.props.match.params.projectId + '/submission/' + submissionPageId);
    };

    onAssignmentFileClicked = () => {
        console.log('FILE');
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

    onModalOpened = (modelType, isFeedbackSRS) => {
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

    onModalOpenedWithComment = (modelType, isFeedbackSRS, commentId, commentText, commentGrade, commentFile) => {
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

    onModalClosed = (modelType, isSuccess) => {
        this.setState({
            [modelType]: false,
        });
        if (!isSuccess) return;
    };

    getInformationPart = () => {
        return (
            <InformationSection
                onCourseClicked={() => this.onCourseClicked(this.state.projectGroup.courseId)}
                courseName={this.state.projectGroup.courseName}
                groupNameElement={this.getGroupNameElement()}
                nameEditIcon={this.getNameEditIcon()}
                memberElements={convertMembersToMemberElement(this.state.projectGroup.members, this.onMemberClicked)}
                informationElement={this.getGroupInformationItem()}
                informationEditIcon={this.getInformationEditIcon()}
            />
        );
    };

    getGroupNameElement = () => {
        let groupNameElement = this.state.projectGroup.name;
        if (this.state.nameEditMode) {
            groupNameElement = <Input onChange={(e) => this.onInputChanged(e, 'newName')} value={this.state.newName} />;
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
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('nameEditMode');
                    }}
                    name={'edit'}
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
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'edit'}
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
                    this.onAssignmentClicked,
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
        if (this.state.projectGroup?.canUserComment) {
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

    getFeedbacksPane = () => {
        const newCommentButton = this.getNewCommentButton();
        const content = (
            <FeedbacksPane
                feedbacksAccordion={getFeedbacksAsAccordion(
                    this.state.feedbacks,
                    this.state.projectGroup?.isTAorInstructor,
                    this.onModalOpenedWithComment,
                    this.onAuthorClicked,
                    this.props.userId
                )}
                newCommentButton={newCommentButton}
            />
        );
        return {
            title: 'Feedbacks',
            content: content,
        };
    };

    getPaneElements = () => {
        return [this.getAssignmentPane(), this.getGradesPane(), this.getFeedbacksPane()];
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
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                />
                <EditCommentModal
                    isOpen={this.state.isEditFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isEditFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                />
                <DeleteCommentModal
                    isOpen={this.state.isDeleteFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isDeleteFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                />
            </>
        );
    };

    render() {
        return (
            <div class="ui centered grid">
                <div class="row">
                    <div class="four wide column">
                        <Segment>{this.getInformationPart()}</Segment>
                    </div>
                    <div class="twelve wide column">
                        {!this.props.match.params.submissionPageId ? (
                            <Tab tabPanes={this.getPaneElements()} />
                        ) : (
                            <ProjectSubmission
                                projectName={this.state.projectGroup?.name}
                                projectId={this.props.match.params.projectId}
                                submissionPageId={this.props.match.params.submissionPageId}
                                userId={this.state.user?.userId}
                            />
                        )}
                    </div>
                </div>
                {this.getModals()}
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
        caption: 'Please download the complete feedback file',
        file: 'dummyFile',
        date: '11 March 2021',
        commentId: 1,
        grade: 9.5,
    },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 3,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    TAComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 4,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
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
            date: '11 March 2021',
            commentId: 5,
            userId: 1,
            userGroupName: 'ClassRoom Helper',
            userGroupId: 5,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
            userGroupName: 'ProjectManager',
            userGroupId: 4,
        },
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
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
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};
