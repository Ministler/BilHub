import {
    convertSRSFeedbackToSRSCardElement,
    convertFeedbacksToFeedbackList,
    convertRequestsToRequestsList,
    convertNewFeedbacksToFeedbackList,
} from '../CardGroup';
import { convertSubmissionsToSubmissionElement } from '../BriefList';
import { MyAccordion } from './AccordionUI';
import { GradesTabel } from '../Statistics';

export const getFeedbacksAsAccordion = (
    feedbacks,
    isTAorInstructor,
    onModalOpenedWithComments,
    onAuthorClicked,
    userId,
    onModalOpened
) => {
    const accordionElements = [
        {
            title: 'SRS Feedback',
            content: convertSRSFeedbackToSRSCardElement(
                feedbacks?.SRSResult,
                isTAorInstructor,
                onModalOpenedWithComments,
                onAuthorClicked,
                onModalOpened
            ),
        },
        {
            title: 'Instructor Feedbacks',
            content: convertFeedbacksToFeedbackList(
                feedbacks?.InstructorComments,
                onModalOpenedWithComments,
                onAuthorClicked,
                userId
            ),
        },
        {
            title: 'TA Feedbacks',
            content: convertFeedbacksToFeedbackList(
                feedbacks?.TAComments,
                onModalOpenedWithComments,
                onAuthorClicked,
                userId
            ),
        },
        {
            title: 'Student Feedbacks',
            content: convertFeedbacksToFeedbackList(
                feedbacks?.StudentComments,
                onModalOpenedWithComments,
                onAuthorClicked,
                userId
            ),
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};

export const getNewFeedbacksAsAccordion = (feedbacks, onSubmissionClicked, onProjectClicked) => {
    const accordionElements = [
        {
            title: 'SRS Feedback',
            content: convertNewFeedbacksToFeedbackList(feedbacks?.SRSResults, onSubmissionClicked, onProjectClicked),
        },
        {
            title: 'Instructor Feedbacks',
            content: convertNewFeedbacksToFeedbackList(
                feedbacks?.InstructorComments,
                onSubmissionClicked,
                onProjectClicked
            ),
        },
        {
            title: 'TA Feedbacks',
            content: convertNewFeedbacksToFeedbackList(feedbacks?.TAFeedbacks, onSubmissionClicked, onProjectClicked),
        },
        {
            title: 'Student Feedbacks',
            content: convertNewFeedbacksToFeedbackList(
                feedbacks?.StudentsFeedbacks,
                onSubmissionClicked,
                onProjectClicked
            ),
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};

export const getRequestsAsAccordion = (
    requests,
    requestsType,
    onUserClicked,
    onRequestApproved,
    onRequestDisapproved
) => {
    const accordionElements = [
        {
            title: 'Pending',
            content: convertRequestsToRequestsList(
                requests?.pending,
                requestsType,
                'pending',
                onUserClicked,
                onRequestApproved,
                onRequestDisapproved
            ),
        },
        {
            title: 'Unresolved',
            content: convertRequestsToRequestsList(requests?.unresolved, requestsType, 'unresolved', onUserClicked),
        },
        {
            title: 'Resolved',
            content: convertRequestsToRequestsList(requests?.resolved, requestsType, 'resolved', onUserClicked),
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};

export const getSubmissionsAsAccordion = (submissions, onSubmissionPageClicked, onSubmissionFileClicked) => {
    const accordionElements = [
        {
            title: 'Graded',
            content: convertSubmissionsToSubmissionElement(
                submissions.graded,
                onSubmissionPageClicked,
                onSubmissionFileClicked
            ),
        },
        {
            title: 'Submitted',
            content: convertSubmissionsToSubmissionElement(
                submissions.submitted,
                onSubmissionPageClicked,
                onSubmissionFileClicked
            ),
        },
        {
            title: 'Not Submitted',
            content: convertSubmissionsToSubmissionElement(
                submissions.notSubmitted,
                onSubmissionPageClicked,
                onSubmissionFileClicked
            ),
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};

export const getAssignmentStatistics = (props) => {
    console.log('tried to print stat');
    const accordionElements = [
        {
            title: 'Table',
            content: <>{GradesTabel(props)}</>,
        },
        {
            title: 'Groups vs Grade Graphic',
            content: 'graph',
        },
        {
            title: 'Grade vs Group Number Graphic',
            content: 'graph',
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};

export const getCourseStatistics = (props) => {
    console.log('tried to print stat');
    const accordionElements = [
        {
            title: 'Table',
            content: 'table stat',
        },
        {
            title: 'Groups vs Grade Graphic',
            content: 'graph',
        },
        {
            title: 'Grade vs Group Number Graphic',
            content: 'graph',
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};
