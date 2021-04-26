import {
    convertSRSFeedbackToSRSCardElement,
    convertFeedbacksToFeedbackList,
    convertRequestsToRequestsList,
    convertNewFeedbacksToFeedbackList,
} from '../CardGroup';
import { MyAccordion } from './AccordionUI';

export const getFeedbacksAsAccordion = (feedbacks, isTAorInstructor, onOpenModal, onAuthorClicked, userId) => {
    const accordionElements = [
        {
            title: 'SRS Feedback',
            content: convertSRSFeedbackToSRSCardElement(
                feedbacks?.SRSResult,
                isTAorInstructor,
                onOpenModal,
                onAuthorClicked
            ),
        },
        {
            title: 'Instructor Feedbacks',
            content: convertFeedbacksToFeedbackList(
                feedbacks?.InstructorComments,
                onOpenModal,
                onAuthorClicked,
                userId
            ),
        },
        {
            title: 'TA Feedbacks',
            content: convertFeedbacksToFeedbackList(feedbacks?.TAComments, onOpenModal, onAuthorClicked, userId),
        },
        {
            title: 'Student Feedbacks',
            content: convertFeedbacksToFeedbackList(feedbacks?.StudentComments, onOpenModal, onAuthorClicked, userId),
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};

export const getNewFeedbacksAsAccordion = (
    feedbacks,
    onUserClicked,
    onSubmissionClicked,
    onProjectClicked,
    onCourseClicked
) => {
    const accordionElements = [
        {
            title: 'SRS Feedback',
            content: convertNewFeedbacksToFeedbackList(
                feedbacks?.SRSResults,
                onUserClicked,
                onSubmissionClicked,
                onProjectClicked,
                onCourseClicked
            ),
        },
        {
            title: 'Instructor Feedbacks',
            content: convertNewFeedbacksToFeedbackList(
                feedbacks?.InstructorComments,
                onUserClicked,
                onSubmissionClicked,
                onProjectClicked,
                onCourseClicked
            ),
        },
        {
            title: 'TA Feedbacks',
            content: convertNewFeedbacksToFeedbackList(
                feedbacks?.TAFeedbacks,
                onUserClicked,
                onSubmissionClicked,
                onProjectClicked,
                onCourseClicked
            ),
        },
        {
            title: 'Student Feedbacks',
            content: convertNewFeedbacksToFeedbackList(
                feedbacks?.StudentsFeedbacks,
                onUserClicked,
                onSubmissionClicked,
                onProjectClicked,
                onCourseClicked
            ),
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};

export const getRequestsAsAccordion = (
    requests,
    requestsType,
    onUserClicked,
    onCourseClicked,
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
                onCourseClicked,
                onRequestApproved,
                onRequestDisapproved
            ),
        },
        {
            title: 'Unresolved',
            content: convertRequestsToRequestsList(
                requests?.unresolved,
                requestsType,
                'unresolved',
                onUserClicked,
                onCourseClicked
            ),
        },
        {
            title: 'Resolved',
            content: convertRequestsToRequestsList(
                requests?.resolved,
                requestsType,
                'resolved',
                onUserClicked,
                onCourseClicked
            ),
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};
