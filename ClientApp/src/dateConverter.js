export function convertDate(date){
    if (!date.includes("T")){
        return date
    }

    let strs = date.split("T")
    let datePart = strs[0]
    datePart = datePart.split("-").reverse().join(".")
    let dayPart = strs[1]
    dayPart = dayPart.split(":")[0] + ":" +  dayPart.split(":")[1]
    return datePart + " " + dayPart
}