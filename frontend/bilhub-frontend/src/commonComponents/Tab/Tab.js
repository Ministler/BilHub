import React from 'react';
import { Tab } from 'semantic-ui-react';

export const MyTab = (props) => {
    const panes = props.panes;
    const paneElements = panes.map((pane) => {
        return {
            menuItem: pane.title,
            render: () => <Tab.Pane>{pane.content}</Tab.Pane>,
        };
    });
    return <Tab panes={paneElements} />;
};
