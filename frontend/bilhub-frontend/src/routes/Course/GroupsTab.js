import React, { Component } from 'react';
import { Grid, Segment, Tab, Icon } from 'semantic-ui-react';
import './Course.css';
export const GroupsTab = (props) => {
    const divGroup = dummyGroupsFormed.map((group, index) => {
        console.log(index);
        return <span>{group[0]}</span>;
    });

    return <>{divGroup}</>;
    // <Segment className="OpacitySegment">
    //     <p>Groups</p>
    // </Segment>
};

const dummyGroupsFormed = [
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
];
