export async function createItem(item, token){
    return await fetch(process.env.REACT_APP_ASP_LINK+"/items", {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
}

export async function readItem(id, token){
    return await fetch(process.env.REACT_APP_ASP_LINK+"/items/"+id,
        {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
}

export async function updateItem(item, token){
    return await fetch(process.env.REACT_APP_ASP_LINK+"/items", {
        method: "PUT",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
}

export async function deleteItem(id, token){
    return await fetch(process.env.REACT_APP_ASP_LINK+"/items/"+id, {
        headers: {
            'Authorization': `Bearer ${token}`
        },
        method: "DELETE"
    })
}