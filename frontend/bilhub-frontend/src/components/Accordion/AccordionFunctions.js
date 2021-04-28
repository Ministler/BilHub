import {
    convertSRSFeedbackToSRSCardElement,
    convertFeedbacksToFeedbackList,
    convertRequestsToRequestsList,
    convertNewFeedbacksToFeedbackList,
} from '../CardGroup';
import { convertSubmissionsToSubmissionElement } from '../BriefList';
import { MyAccordion } from './AccordionUI';
import { GradesTabel, GroupNoGradeGraph } from '../Statistics';
import React from 'react';

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
            content: <GradesTabel graders={props.graders} groups={props.groups} />,
        },
        {
            title: 'Groups vs Grade Graphic',
            content: <GroupNoGradeGraph />,
        },
        {
            title: 'Grade vs Group Number Graphic',
            content: 'graph',
        },
    ];

    return <MyAccordion accordionSections={accordionElements} />;
};
