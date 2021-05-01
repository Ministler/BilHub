export function inputDateToDateObject(inputDate) {
    let dateArr = inputDate.split(/-/);
    dateArr[2] = dateArr[2].split(/T/);
    dateArr[2][1] = dateArr[2][1].split(/:/);
    return new Date(dateArr[0], dateArr[1], dateArr[2][0], dateArr[2][1][0], dateArr[2][1][1]);
}

export function dateObjectToInputDate(dateObject) {
    return (
        '' +
        dateObject.getFullYear() +
        '-' +
        (dateObject.getMonth() / 10 < 1 ? '0' + dateObject.getMonth() : dateObject.getMonth()) +
        '-' +
        (dateObject.getDate() / 10 < 1 ? '0' + dateObject.getDate() : dateObject.getDate()) +
        'T' +
        (dateObject.getHours() / 10 < 1 ? '0' + dateObject.getHours() : dateObject.getHours()) +
        ':' +
        (dateObject.getMinutes() / 10 < 1 ? '0' + dateObject.getMinutes() : dateObject.getMinutes())
    );
}

export function dateObjectToString(dateObject) {
    return (
        '' +
        (dateObject.getDate() / 10 < 1 ? '0' + dateObject.getDate() : dateObject.getDate()) +
        '.' +
        (dateObject.getMonth() / 10 < 1 ? '0' + dateObject.getMonth() : dateObject.getMonth()) +
        '.' +
        dateObject.getFullYear() +
        ' ' +
        (dateObject.getHours() / 10 < 1 ? '0' + dateObject.getHours() : dateObject.getHours()) +
        ':' +
        (dateObject.getMinutes() / 10 < 1 ? '0' + dateObject.getMinutes() : dateObject.getMinutes())
    );
}
