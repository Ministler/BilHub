import React from 'react';

import { convertMembersToMemberElement } from '../../../components';

import { Button, Modal, Icon, TextArea } from 'semantic-ui-react';

export const InformationSection = (props) => {
    return (
        <div>
            <div>
                <h1>
                    {props.courseName}
                    {props.courseSettingsIcon}
                </h1>

                <h3>{props.description}</h3>
            </div>
            <div>
                <h2>Instructor</h2>
                {convertMembersToMemberElement(props.instructors, props.onUserClicked)}
            </div>
            <div>
                <h2>TA's</h2>
                {convertMembersToMemberElement(props.TAs, props.onUserClicked)}
            </div>
            <div>
                <h3>Information</h3>
                {props.informationEditIcon}
                <p>{props.informationElement}</p>
            </div>
        </div>
    );
};
