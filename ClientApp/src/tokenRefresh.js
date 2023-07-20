import {Mutex} from "async-mutex";

export default async function refresh(){
    const mutex = new Mutex();
    return mutex
        .acquire()
        .then(function (release){
    let current = sessionStorage.getItem("todoJWT")
    let data = {
        accessToken: sessionStorage.getItem("todoJWT"),
        refreshToken: sessionStorage.getItem("todoJWTRefresh")
    }
    if (data.refreshToken === "" || data.accessToken === ""){
        release()
        return false
    }
    return fetch(process.env.REACT_APP_ASP_LINK+"/tokens/refresh", {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
    }).then(async (response) => {
        if (response.ok){
            let r = await response.json()
            sessionStorage.setItem("todoJWT", r.accessToken)
            sessionStorage.setItem("todoJWTRefresh", r.refreshToken)
            release()
            return true
        }
        else if(response.status === 400){
            if (sessionStorage.getItem("todoJWT") !== current){
                release()
                return true
            }

            console.log(await response.text())
            sessionStorage.setItem("todoJWT","")
            sessionStorage.setItem("todoJWTRefresh", "")
            sessionStorage.setItem("todoUsername", "")
        }
        release()
         return false
    })
        })
}