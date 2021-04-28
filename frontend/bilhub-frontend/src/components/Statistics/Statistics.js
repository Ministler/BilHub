import _ from 'lodash';
import { Table } from 'semantic-ui-react';
import React from 'react';
import Chart from 'react-google-charts';

function tableReducer(state, action) {
    switch (action.type) {
        case 'CHANGE_SORT':
            if (state.column === action.column) {
                return {
                    ...state,
                    data: state.data.slice().reverse(),
                    direction: state.direction === 'ascending' ? 'descending' : 'ascending',
                };
            }

            return {
                column: action.column,
                data: _.sortBy(state.data, [action.column]),
                direction: 'ascending',
            };
        default:
            throw new Error();
    }
}

export const GradesTabel = (props) => {
    var tempData = [];
    for (var i = 0; i < props.groups.length; i++) {
        tempData.push({ group: props.groups[i].name });
        for (var k = 0; k < props.graders.length; k++) {
            tempData[i][k] = props.groups[i].grades[k];
        }
    }
    const [state, dispatch] = React.useReducer(tableReducer, {
        column: null,
        data: tempData,
        direction: null,
    });

    const { column, data, direction } = state;
    return (
        <Table sortable celled fixed>
            <Table.Header>
                <Table.Row>
                    <Table.HeaderCell
                        sorted={column === 'group' ? direction : null}
                        onClick={() => dispatch({ type: 'CHANGE_SORT', column: 'group' })}>
                        Groups
                    </Table.HeaderCell>
                    {}
                    {props.graders.map((grader, index) => (
                        <Table.HeaderCell
                            sorted={column === index ? direction : null}
                            onClick={() => dispatch({ type: 'CHANGE_SORT', column: index })}>
                            {grader}
                        </Table.HeaderCell>
                    ))}
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {data.map((group) => (
                    <Table.Row>
                        <Table.Cell>{group['group']}</Table.Cell>
                        {Object.keys(group).map(
                            (element) => element !== 'group' && <Table.Cell>{group[element]}</Table.Cell>
                        )}
                    </Table.Row>
                ))}
            </Table.Body>
        </Table>
    );
};
const data = [
    ['Element', 'Density', { role: 'style' }],
    ['Copper', 8.94, '#b87333'], // RGB value
    ['Silver', 10.49, 'silver'], // English color name
    ['Gold', 19.3, 'gold'],
    ['Platinum', 21.45, 'color: #e5e4e2'], // CSS-style declaration
];
export const GroupNoGradeGraph = (props) => {
    return (
        <div className="App">
            <Chart chartType="ColumnChart" width="100%" height="400px" data={data} />
        </div>
    );
};

export const GradeGroupGraph = (props) => {};
