import React, {useState, useEffect, useContext, useCallback} from 'react';
import { useNavigate } from "react-router-dom";
import './Create.css'
import { Error } from "../Error";
import { TokenContext } from "../../App";
import {createList, readList} from "./listOperations";

export function CreateList(){
    const [title, setTitle] = useState("");
    const [archived, setArchived] = useState(false);
    const navigate = useNavigate();
    const [errors, setErrors] = useState([])
    const { token } = useContext(TokenContext);

    const check = useCallback(async () => {
        await readList(1, token).then(async (response) => {
                if (response.status === 401){
                    navigate("/login")
                }
            })
    }, [token, navigate])

    useEffect(() => {
        check().then()
    }, [check])

    async function submitHandler(e){
        e.preventDefault()
        let data = {
            title: title,
            isArchived: archived
        }
        await createList(data, token).then(async (response) => {
            if (response.ok){
                navigate(-1)
            }else if (response.status === 401){
                navigate("/login")
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
