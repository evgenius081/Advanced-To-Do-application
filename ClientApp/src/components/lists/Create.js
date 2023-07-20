import React, {useState, useEffect, useCallback} from 'react';
import { useNavigate } from "react-router-dom";
import './Create.css'
import { Error } from "../Error";
import {createList, readList} from "./listOperations";
import jwt_decode from "jwt-decode";
import refresh from "../../tokenRefresh";

export function CreateList(){
    const [title, setTitle] = useState("");
    const [archived, setArchived] = useState(false);
    const navigate = useNavigate();
    const [errors, setErrors] = useState([])

    const check = useCallback(async () => {
        
        await readList(1).then(async (response) => {
            if (response.status === 401){
                if (await refresh()) {
                    await check()
                }else{
                    navigate("/login")
                }
            }
        })
    }, [navigate])

    useEffect(() => {
        check().then()
    }, [check])

    async function submitHandler(e){
        let token = sessionStorage.getItem("todoJWT");
        e.preventDefault()
        let data = {
            title: title,
            isArchived: archived,
            userID: parseInt(jwt_decode(token).nameid)
        }
        await createList(data).then(async (response) => {
            if (response.ok){
                navigate(-1)
            }else if (response.status === 401){
                if (await refresh()) {
                    await submitHandler(e)
                }else{
                    navigate("/login")
                }
            }else if (response.status === 400){
                let data = await response.json()
                setErrors(await data.errors.Title)
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    return (
        <form>
            <div className="form-group">
                <label htmlFor="list-title-input">Title</label>
                <input type="text" className="form-control" id="list-title-input" aria-describedby="list-title-help"
                       value={title} onChange={e => setTitle(e.target.value)} placeholder="Enter title" />
            </div>
            <div className="form-group">
                <input className="form-check-input" type="checkbox" onChange={() => setArchived(!archived)} checked={archived} id="list-archived-input" />
                <label className="form-check-label" htmlFor="list-archived-input">
                    Archive
                </label>
            </div>
            <Error errors={errors}/>
            <button type="submit" id="submit" className="btn btn-primary" onClick={submitHandler}>Submit</button>
        </form>
    )
    }
