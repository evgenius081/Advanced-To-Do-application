export async function createList(list, token){
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

export async function readList(id, token){
    return await fetch(process.env.REACT_APP_ASP_LINK+"/lists/"+id,
        {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
}

export async function updateList(list, token){
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

export async function deleteList(id, token){
    return await fetch(process.env.REACT_APP_ASP_LINK+"/lists/"+id, {
        headers: {
            'Authorization': `Bearer ${token}`
        },
        method: "DELETE"
    })
}