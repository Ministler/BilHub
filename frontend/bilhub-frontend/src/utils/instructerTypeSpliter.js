export function instructerTypeSplit(dataList) {
    let instructerList = [];
    let taList = [];

    for (let i = 0; i < dataList.length; i++) {
        if(dataList[i].userType === "Instructor")
            instructerList.push(dataList[i]);
        else
            taList.push(dataList[i]);                            
    }

    return {instructerList, taList};
}