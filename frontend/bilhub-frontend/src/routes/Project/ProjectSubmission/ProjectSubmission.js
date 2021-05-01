import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Icon, Button } from 'semantic-ui-react';
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
import { dateObjectToString } from '../../../utils';

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
            currentMaxFeedbackGrade: 10,
            currentFeedbackId: 0,
        };
    }

    onAssignmentFileClicked = () => {
        console.log('file');
    };

    onSubmissionFileClicked = () => {
        console.log('file');
    };

    onSubmissionModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        if (modalType === 'isAddSubmissionOpen') {
            request = {
                submissionId: this.props.match.params.submissionId,
                file: this.state.submissionFile,
                submissionCaption: this.state.submissionCaption,
            };
        } else if (modalType === 'isEditSubmissionOpen') {
            request = {
                submissionId: this.props.match.params.submissionId,
                file: this.state.submissionFile,
                submissionCaption: this.state.submissionCaption,
            };
        } else if (modalType === 'isDeleteSubmissionOpen') {
            request = {
                submissionId: this.props.match.params.submissionId,
            };
        }

        console.log(request);
    };

    onModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        if (this.state.isFeedbackSRS) {
            if (modalType === 'isGiveFeedbackOpen') {
                request = {
                    grade: this.state.currentFeedbackGrade,
                    maxGrade: this.state.currentMaxFeedbackGrade,
                    userId: this.props.userId,
                    submissionId: this.props.match.params.submissionId,
                };
            } else if (modalType === 'isEditFeedbackOpen') {
                request = {
                    grade: this.state.currentFeedbackGrade,
                    maxGrade: this.state.currentMaxFeedbackGrade,
                    commentId: this.state.currentFeedbackId,
                    userId: this.props.userId,
                };
            } else if (modalType === 'isDeleteFeedbackOpen') {
                request = {
                    commentId: this.state.currentFeedbackId,
                    userId: this.props.userId,
                };
            }
        } else {
            if (modalType === 'isGiveFeedbackOpen') {
                request = {
                    newGrade: this.state.currentFeedbackGrade,
                    newText: this.state.currentFeedbackText,
                    newFile: this.state.currentFeedbackFile,
                    userId: this.props.userId,
                    submissionId: this.props.match.params.submissionId,
                };
            } else if (modalType === 'isEditFeedbackOpen') {
                request = {
                    newGrade: this.state.currentFeedbackGrade,
                    newText: this.state.currentFeedbackText,
                    newFile: this.state.currentFeedbackFile,
                    commentId: this.state.currentFeedbackId,
                    userId: this.props.userId,
                };
            } else if (modalType === 'isDeleteFeedbackOpen') {
                request = {
                    commentId: this.state.currentFeedbackId,
                    userId: this.props.userId,
                };
            }
        }

        console.log(request);
    };

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

    onSubmissionModalOpened = (modalType) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: '',
                submissionFile: 'empty',
            });
        }
    };

    onExistingSubmissionModalOpened = (modalType, submissionCaption, submissionFile) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: submissionCaption,
                submissionFile: submissionFile,
            });
        }
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

    onCurrentFeedbackMaxGradeChanged = (e) => {
        e.preventDefault();
        this.setState({
            currentMaxFeedbackGrade: e.target.value,
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
                currentMaxFeedbackGrade: 10,
                currentFeedbackFile: 'empty',
                currentFeedbackText: '',
            });
        }
    };

    onModalOpenedWithCommentOpened = (
        modalType,
        isFeedbackSRS,
        commentId,
        commentText,
        commentGrade,
        commentFile,
        SRSMaxGrade
    ) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
                currentMaxFeedbackGrade: SRSMaxGrade,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
                currentMaxFeedbackGrade: 10,
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

    getSubmissionButtons = () => {
        if (this.state.submissionPage?.isInGroup) {
            if (!this.state.submissionPage.hasSubmission) {
                return (
                    <Button onClick={() => this.onSubmissionModalOpened('isAddSubmissionOpen')}>
                        Add New Submission
                    </Button>
                );
            } else if (this.state.submissionPage.isEdittable) {
                return (
                    <>
                        <Button
                            onClick={() =>
                                this.onExistingSubmissionModalOpened(
                                    'isEditSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.hasFile
                                )
                            }>
                            Edit Submission
                        </Button>
                        <Button
                            onClick={() =>
                                this.onExistingSubmissionModalOpened(
                                    'isDeleteSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.hasFile
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
                    this.onModalOpenedWithCommentOpened,
                    this.onAuthorClicked,
                    this.props.userId,
                    this.onModalOpened
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
            date:
                (typeof this.state.assignment.publishmentDate === 'object'
                    ? dateObjectToString(this.state.assignment.publishmentDate)
                    : this.state.assignment.publishmentDate) +
                ' / ' +
                (typeof this.state.assignment.dueDate === 'object'
                    ? dateObjectToString(this.state.assignment.dueDate)
                    : this.state.assignment.dueDate),
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
                        isLate={this.state.submissionPage?.isLate}
                        onAssignmentFileClicked={this.onAssignmentFileClicked}
                        buttons={submissionButtons}
                    />
                );
            } else {
                submissionPane = (
                    <SubmissionPane
                        assignment={assignment}
                        submission={this.state.submission}
                        isLate={this.state.submissionPage?.isLate}
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
                    instructions={this.state.assignment.submissionInfo}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isAddSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChanged(e)}
                />
                <EditSubmissionModal
                    isOpen={this.state.isEditSubmissionOpen}
                    instructions={this.state.assignment.submissionInfo}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isEditSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChanged(e)}
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
                    maxGrade={this.state.currentMaxFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                    onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged(e)}
                />
                <EditCommentModal
                    isOpen={this.state.isEditFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isEditFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                    onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged(e)}
                />
                <DeleteCommentModal
                    isOpen={this.state.isDeleteFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isDeleteFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                />
            </>
        );
    };

    render() {
        return (
            <div class="inline">
                <Icon
                    onClick={this.onReturnProjectPage}
                    size="big"
                    name="angle left"
                    color="blue"
                    style={{ display: 'inline' }}
                />
                <p
                    onClick={this.onReturnProjectPage}
                    style={{ display: 'inline', fontSize: '16px', fontWeight: 'bold', color: 'rgb(33, 133, 208)' }}>
                    Back To Assigments
                </p>
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
    publishmentDate: new Date(2023, 3, 13, 12, 0),
    dueDate: new Date(2025, 4, 16, 23, 59),
    file: 'deneme',
    submissionInfo: 'Please name your submission file as: surname_name_id_section.pdf',
};

const dummySubmissionPage = {
    isSubmissionAnonim: false,
    isInGroup: true,
    isTAorInstructor: false,
    canUserComment: true,
    hasSubmission: true,
    isLate: true,
    isEdittable: false,
};

const dummySubmission = {
    caption:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Provident dicta dignissimos dolore quo iure, et ipsam corporis accusamus ad eligendi, inventore consequatur, repellendus laboriosam vitae sed quam fugit. Omnis dignissimos eos libero facilis quisquam quidem. Labore veritatis eaque non vero asperiores, soluta, qui nisi adipisci, fugit corrupti praesentium voluptatem enim?',
    file: 'file',
    date: new Date(2025, 4, 16, 12, 31),
    submissionId: 1,
};

const dummyGrades = {
    persons: [
        {
            name: 'Eray Tüzün',
            type: 'Project Instructor',
            grade: '9.5',
            userId: 'dD3wUcJiDHTM9aDs8livI9HpY3h2',
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
        grade: '9.5',
        maxGrade: '11',
    },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 3, 11, 12, 0),
            commentId: 3,
            userId: 'dD3wUcJiDHTM9aDs8livI9HpY3h2',
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
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
            date: new Date(2021, 3, 11, 12, 0),
            commentId: 4,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    StudentComments: [
        {
            //name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            //grade: '9.5',
            date: new Date(2021, 3, 11, 12, 0),
            commentId: 5,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
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
            date: new Date(2021, 3, 11, 12, 0),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
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
            date: new Date(2021, 3, 11, 12, 0),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};
