import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Icon, Button, Grid } from 'semantic-ui-react';
import { withRouter } from 'react-router-dom';
import {
    NewCommentModal,
    EditCommentModal,
    DeleteCommentModal,
    NewSubmissionModal,
    EditSubmissionModal,
    DeleteSubmissionModal,
} from '../ProjectComponents';
import {
    Tab,
    getGradeTable,
    getFeedbacksAsAccordion,
    GradePane,
    FeedbacksPane,
    SubmissionPane,
} from '../../../components';

class ProjectAssignment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            assignment: {},
            submissionPage: {},
            submission: null,
            grades: null,
            feedbacks: [],

            // State modals regarding submission
            isAddSubmissionOpen: false,
            isEditSubmissionOpen: false,
            isDeleteSubmissionOpen: false,
            submissionFile: null,
            submissionCaption: '',

            // States modals regarding feedbacks
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
            assignment: dummyAssignment,
            grades: dummyGrades,
            feedbacks: dummyFeedbacks,
            submission: dummySubmission,
            submissionPage: dummySubmissionPage,
        });
    }

    onReturnProjectPage = () => {
        this.props.history.replace('/project/' + this.props.projectId);
    };

    onAssignmentFileClicked = () => {
        console.log('file');
    };

    onSubmissionFileClicked = () => {
        console.log('file');
    };

    onSubmissionmodalOpened = (modalType) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: '',
                submissionFile: 'empty',
            });
        }
    };

    onExistingSubmissionmodalOpened = (modalType, submissionCaption, submissionFile) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: submissionCaption,
                submissionFile: submissionFile,
            });
        }
    };

    onSubmissionModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;
    };
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

    onSubmissionCaptionChanged = (e) => {
        e.preventDefault();
        this.setState({
            submissionCaption: e.target.value,
        });
    };

    // modals
    onModalOpened = (modalType, isFeedbackSRS) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modalType) {
            this.setState({
                [modalType]: true,
                currentFeedbackGrade: 10,
                currentFeedbackFile: 'empty',
                currentFeedbackText: '',
            });
        }
    };

    onmodalOpenedWithCommentOpened = (modalType, isFeedbackSRS, commentId, commentText, commentGrade, commentFile) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modalType) {
            this.setState({
                currentFeedbackId: commentId,
                [modalType]: true,
                currentFeedbackText: commentText,
                currentFeedbackGrade: commentGrade,
                currentFeedbackFile: commentFile,
            });
        }
    };

    onSubmissionModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;
    };

    onModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;
    };

    getSubmissionButtons = () => {
        if (this.state.submissionPage?.isInGroup) {
            if (!this.state.submissionPage.hasSubmission) {
                return (
                    <Button onClick={() => this.onSubmissionmodalOpened('isAddSubmissionOpen')}>
                        Add New Submission
                    </Button>
                );
            } else {
                return (
                    <>
                        <Button
                            onClick={() =>
                                this.openExistingSubmissionmodal(
                                    'isEditSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.file
                                )
                            }>
                            Edit Submission
                        </Button>
                        <Button
                            onClick={() =>
                                this.openExistingSubmissionmodal(
                                    'isDeleteSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.file
                                )
                            }>
                            Delete Submission
                        </Button>
                    </>
                );
            }
        }

        return null;
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
                        this.props.projectName
                    )}
                    finalGrade={this.state.grades?.finalGrade}
                />
            ) : (
                <div>No Grades</div>
            ),
        };
    };

    getNewCommentButton = () => {
        let newCommentButton = null;
        if (this.state.submissionPage?.canUserComment) {
            newCommentButton = (
                <Button
                    content="Give Feedback"
                    labelPosition="right"
                    icon="edit"
                    primary
                    onClick={() => this.onModalOpened('isGiveFeedbackOpen', false)}
                />
            );
        }

        return newCommentButton;
    };

    onAuthorClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    getFeedbacksPane = () => {
        const newCommentButton = this.getNewCommentButton();
        const content = (
            <FeedbacksPane
                feedbacksAccordion={getFeedbacksAsAccordion(
                    this.state.feedbacks,
                    this.state.submissionPage?.isTAorInstructor,
                    this.onmodalOpenedWithCommentOpened,
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
        const assignment = {
            ...this.state.assignment,
            date: this.state.assignment.publishmentDate + ' / ' + this.state.assignment.dueDate,
        };

        const submissionButtons = this.getSubmissionButtons();

        let submissionPane;
        if (this.state.submissionPage?.hasSubmission) {
            if (
                this.state.submissionPage.isSubmissionAnonim &&
                !this.state.submissionPage.isTAorInstructor &&
                !this.state.submissionPage.isInGroup
            ) {
                submissionPane = (
                    <SubmissionPane
                        assignment={assignment}
                        submission={'anonim'}
                        onAssignmentFileClicked={this.onAssignmentFileClicked}
                        buttons={submissionButtons}
                    />
                );
            } else {
                submissionPane = (
                    <SubmissionPane
                        assignment={assignment}
                        submission={this.state.submission}
                        onAssignmentFileClicked={this.onAssignmentFileClicked}
                        onSubmissionFileClicked={this.onSubmissionFileClicked}
                        buttons={submissionButtons}
                    />
                );
            }
        } else {
            submissionPane = (
                <SubmissionPane
                    assignment={assignment}
                    onAssignmentFileClicked={this.onAssignmentFileClicked}
                    buttons={submissionButtons}
                />
            );
        }

        return [
            {
                title: 'Submission',
                content: this.state.submissionPage ? submissionPane : <div>No Such Assignment</div>,
            },
            this.getGradesPane(),
            this.getFeedbacksPane(),
        ];
    };

    // Modals
    getModals = () => {
        return (
            <>
                <NewSubmissionModal
                    isOpen={this.state.isAddSubmissionOpen}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isAddSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChanged(e)}
                />
                <EditSubmissionModal
                    isOpen={this.state.isEditSubmissionOpen}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isEditSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChange(e)}
                />
                <DeleteSubmissionModal
                    isOpen={this.state.isDeleteSubmissionOpen}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isDeleteSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                />
                <NewCommentModal
                    isOpen={this.state.isGiveFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isGiveFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                />
                <EditCommentModal
                    isOpen={this.state.isEditFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isEditFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                />
                <DeleteCommentModal
                    isOpen={this.state.isDeleteFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isDeleteFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                />
            </>
        );
    };

    render() {
        return (
            <div class="inline">
                <Icon onClick={this.onReturnProjectPage} size="big" name="angle left" color="blue" style={{display: 'inline',}}/>
                <p onClick={this.onReturnProjectPage} style={{display: 'inline', fontSize: "16px", fontWeight: "bold", color: "rgb(33, 133, 208)"}}>Back To Assigments</p>
                <Tab tabPanes={this.getPaneElements()} />
                {this.getModals()}
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return { userId: state.userId, token: state.token };
};

export default withRouter(connect(mapStateToProps)(ProjectAssignment));

const dummyAssignment = {
    title: 'CS319-2021Spring / Desing Report Assignment',
    status: 'graded',
    caption:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
    publisher: 'Erdem Tuna',
    publisherId: 1,
    publishmentDate: '13 March 2023 12:00',
    dueDate: '16 April 2025, 23:59',
    file: 'deneme',
    submissionInfo: 'Please name your submission file as: surname_name_id_section.pdf',
};

const dummySubmissionPage = {
    isSubmissionAnonim: false,
    isInGroup: true,
    isTAorInstructor: false,
    canUserComment: true,
    hasSubmission: true,
};

const dummySubmission = {
    caption:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Provident dicta dignissimos dolore quo iure, et ipsam corporis accusamus ad eligendi, inventore consequatur, repellendus laboriosam vitae sed quam fugit. Omnis dignissimos eos libero facilis quisquam quidem. Labore veritatis eaque non vero asperiores, soluta, qui nisi adipisci, fugit corrupti praesentium voluptatem enim?',
    file: 'file',
    date: '16 April 2025, 12:31',
    submissionId: 1,
};

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
        userId: 1,
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
