export async function createList(list){
    let token = sessionStorage.getItem("todoJWT");
    return await fetch(process.env.REACT_APP_ASP_LINK+"/lists", {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(list)
    })
}

export async function readList(id){
    let token = sessionStorage.getItem("todoJWT");
    return await fetch(process.env.REACT_APP_ASP_LINK+"/lists/"+id,
        {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
}

export async function updateList(list){
    let token = sessionStorage.getItem("todoJWT");
    return await fetch(process.env.REACT_APP_ASP_LINK+"/lists", {
        method: "PUT",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(list)
    })
}

export async function deleteList(id){
    let token = sessionStorage.getItem("todoJWT");
    return await fetch(process.env.REACT_APP_ASP_LINK+"/lists/"+id, {
        headers: {
            'Authorization': `Bearer ${token}`
        },
        method: "DELETE"
    })
}