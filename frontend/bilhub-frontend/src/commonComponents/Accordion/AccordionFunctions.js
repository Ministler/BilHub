import { convertSRSFeedbackToSRSCardElement, convertFeedbacksToFeedbackList } from '../CardGroup';
import { MyAccordion } from './AccordionUI';

export const getFeedbacksAsAccordion = (
    feedbacks,
    isTAorInstructor,
    onOpenModal,
    onAuthorClicked,
    userId,
    onAccordionClick,
    accordionActiveIndex
) => {
    const accordionElements = [
        {
            title: 'SRS Feedback',
            content: convertSRSFeedbackToSRSCardElement(feedbacks?.SRSResult, isTAorInstructor, onOpenModal),
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
            content: convertFeedbacksToFeedbackList(
                feedbacks?.InstructorComments,
                onOpenModal,
                onAuthorClicked,
                userId
            ),
        },
        {
            title: 'Student Feedbacks',
            content: convertFeedbacksToFeedbackList(
                feedbacks?.InstructorComments,
                onOpenModal,
                onAuthorClicked,
                userId
            ),
        },
    ];

    return (
        <MyAccordion
            onClick={onAccordionClick}
            activeIndex={accordionActiveIndex}
            accordionSections={accordionElements}
        />
    );
};
