import React from 'react';
import { Table } from 'semantic-ui-react';

export const MyTable = (props) => {
    const headerNames = props.headerNames;
    const headerCells = headerNames.map((headerName) => {
        return <Table.HeaderCell>{headerName}</Table.HeaderCell>;
    });

    const bodyRowsData = props.bodyRowsData;
    const bodyRows = bodyRowsData.map((bodyRowData) => {
        const row = bodyRowData.map((data) => {
            return <Table.Cell>{data}</Table.Cell>;
        });
        return <Table.Row>{row}</Table.Row>;
    });
    return (
        <Table celled>
            <Table.Header>
                <Table.Row>{headerCells}</Table.Row>
            </Table.Header>
            <Table.Body>{bodyRows}</Table.Body>
        </Table>
    );
};
