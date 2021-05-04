export function instructerTypeSplitInstructers(dataList) {
    let instructerList = [];

    for (let i = 0; i < dataList?.length; i++) {
        if (dataList[i].userType === 'Instructor') instructerList.push(dataList[i]);
    }

    return instructerList;
}

export function instructerTypeSplitgeTAs(dataList) {
    let taList = [];

    for (let i = 0; i < dataList?.length; i++) {
        if (dataList[i].userType !== 'Instructor') taList.push(dataList[i]);
    }

    return taList;
}
