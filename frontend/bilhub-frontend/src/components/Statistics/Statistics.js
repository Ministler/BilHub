import _ from 'lodash';
import { Table } from 'semantic-ui-react';
import React from 'react';

/*function tableReducer(state, action) {
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
}*/

export const GradesTabel = (props) => {
    /*const [state, dispatch] = React.useReducer(tableReducer, {
        column: null,
        data: props,
        direction: null,
    });*/
    //const { column, data, direction } = state;
    return (
        <Table sortable celled fixed>
            <Table.Header>
                <Table.Row>
                    <Table.HeaderCell
                    //sorted={column === 'groups' ? direction : null}
                    //onClick={() => dispatch({ type: 'CHANGE_SORT', column: 'group' })}>
                    >
                        Groups
                    </Table.HeaderCell>
                    {props.graders.map((grader) => (
                        <Table.HeaderCell /*
                            sorted={column === grader ? direction : null}
                            onClick={() => dispatch({ type: 'CHANGE_SORT', column: grader })}*/
                        >
                            {grader}
                        </Table.HeaderCell>
                    ))}
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {props.groups.map(({ name, grades }) => (
                    <Table.Row key={name}>
                        <Table.Cell>{name}</Table.Cell>
                        {console.log(grades)}
                        {grades.map((grade) => (
                            <Table.Cell>{grade}</Table.Cell>
                        ))}
                    </Table.Row>
                ))}
            </Table.Body>
        </Table>
    );
};

export const GroupNoGradeGraph = (props) => {};

export const GradeGroupGraph = (props) => {};
