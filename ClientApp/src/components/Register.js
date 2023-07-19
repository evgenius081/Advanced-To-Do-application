import React, {useContext, useState} from 'react'
import {Link, useNavigate} from "react-router-dom";
import {Error} from "./Error";
import { TokenContext } from "../App";

export function Register(){
    const [ password, setPassword ] = useState("")
    const [ errors, setErrors ] = useState([])
    let navigate = useNavigate()
    const { setToken } = useContext(TokenContext);
    const [ username, setUsername ] = useState("")

    async function submitHandler(e){
        e.preventDefault()
        let data = {
            "login": username,
            "password": password
        }
        await fetch(process.env.REACT_APP_ASP_LINK+"/users/register", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        })
            .then(async (response) => {
                if (response.ok){
                    let token = await response.json()
                    setToken(token)
                    sessionStorage.setItem("todoUsername", username)
                    sessionStorage.setItem("todoJWT", token)
                    navigate("/")
                }
                else if (response.status === 400){
                    let text = await response.json()
                    setErrors([text])
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }

    return (
        <form>
            <div className="form-group">
                <label htmlFor="username-input">Login</label>
                <input type="text" className="form-control" id="username-input" aria-describedby="username-help"
                       value={username} onChange={e => setUsername(e.target.value)} placeholder="Enter login" />
            </div>
            <div className="form-group">
                <label htmlFor="password-input">Password</label>
                <input type="password" className="form-control" id="password-input" aria-describedby="password-help"
                       value={password} onChange={e => setPassword(e.target.value)} placeholder="Enter password" />
            </div>
            <Error errors={errors}/>
            <button type="submit" id="submit" className="btn btn-primary" onClick={(e) => submitHandler(e)}>Submit</button>
            <Link to="/login" className="user-link">login</Link>
        </form>
    )
}