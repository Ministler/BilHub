import React from 'react';

import { convertMembersToMemberElement } from '../../../components';
import './CourseComponents.css';
import { Button, Modal, Icon, TextArea } from 'semantic-ui-react';

export const InformationSection = (props) => {
    return (
        <div>
            <div>
                <h1 style={{ display: 'inline' }}>{props.courseName}</h1>
                <span className="CourseNameEdit"> {props.courseSettingsIcon} </span>
                <p>{props.description}</p>
            </div>
            <div className="InstructorsBlock">
                <h3>Instructors</h3>
                {convertMembersToMemberElement(props.instructors, props.onUserClicked)}
            </div>
            <div className="TAsBlock">
                <h3>TA's</h3>
                {convertMembersToMemberElement(props.TAs, props.onUserClicked)}
            </div>
            <div className="InformationBlock">
                <h3>Information</h3>
                <p className="InformationText" style={{ display: 'inline-block' }}>
                    {props.informationElement} &nbsp;
                </p>
                {props.informationEditIcon}
            </div>
        </div>
    );
};
