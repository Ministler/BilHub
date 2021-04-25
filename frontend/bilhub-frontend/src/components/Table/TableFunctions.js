import React from 'react';

import { MyTable } from './TableUI';

// For logic regarding grades
export const getGradeTable = (grades, projectAvarageGrade, courseAvarageGrade, projectGroupName) => {
    return (
        <>
            <MyTable bodyRowsData={getTableBodyFromGrades(grades)} headerNames={['User', 'Person', 'Grade']} />
            <MyTable
                bodyRowsData={[[projectAvarageGrade, courseAvarageGrade]]}
                headerNames={[projectGroupName + ' Avarage', 'Course Average']}
            />
        </>
    );
};

const getTableBodyFromGrades = (grades) => {
    const persons = grades.persons;

    const personsData = persons.map((person) => {
        return [person.type, person.name, person.grade];
    });

    if (grades.studentsAverage) {
        personsData.push(['Students Average', '', grades.studentsAverage]);
    }

    return personsData;
};
