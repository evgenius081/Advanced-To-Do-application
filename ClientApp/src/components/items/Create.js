import React, {useCallback, useContext, useEffect, useState} from 'react';
import Dropdown from 'react-bootstrap/Dropdown';
import { Error } from "../Error";

import './Create.css'
import {useNavigate, useParams} from "react-router-dom";
import { TokenContext } from "../../App";
import {readList} from "../lists/listOperations";
import {createItem} from "./itemOperations";

export function CreateItem(){
    let { list_id } = useParams();
    let navigate = useNavigate()
    const statuses = ["Not started", "In process", "Completed"];
    const priorities = ["Hidden", "Standard", "High"]
    const [title, setTitle] = useState("")
    const [description, setDescription] = useState("")
    const [status, setStatus] = useState(0)
    const d = new Date();
    d.setMinutes(d.getMinutes() - d.getTimezoneOffset());
    const [deadline, setDeadline] = useState(d.toISOString().slice(0, 16));
    const [reminded, setReminded] = useState(false)
    const [priority, setPriority] = useState(1)
    const [errors, setErrors] = useState([]);
    const { token, getReminded } = useContext(TokenContext);

    const checkList = useCallback(async () => {
        await readList(list_id, token)
            .then((response) => {
                if (response.status === 401){
                    navigate("login")
                }
                else if (response.status === 404){
                    navigate("/notFound")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }, [navigate, list_id, token])

    useEffect(() => {
        checkList().then()
    }, [checkList])

    async function sendData(e){
        e.preventDefault()
        let data = {
            title: title,
            description: description,
            deadline: deadline,
            createdAt: new Date().toJSON(),
            status: status,
            remind: reminded,
            priority: priority,
            toDoListID: parseInt(list_id)
        }

        await createItem(data, token).then(async (response) => {
            if (response.ok){
                getReminded()
                navigate(-1)
            }
            if (response.status === 400){
                let data = await response.json()
                setErrors(data.errors.Title)
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

        return (
            <form>
                <div className="form-group">
                  <label htmlFor="todo-entry-title">Title</label>
                  <input type="text" className="form-control" id="todo-entry-title" value={title} onChange={e => setTitle(e.target.value)} placeholder="Enter title" required={true}/>
                </div>
                <div className="form-group">
                    <label htmlFor="todo-entry-description">Description</label>
                    <textarea className="form-control" id="todo-entry-description" value={description} onChange={e => setDescription(e.target.value)} placeholder="Enter description"></textarea>
                </div>
                <div className="form-group">
                    <div className="input-group">
                        <div className="form-group">
                            <label htmlFor="deadline-input">Deadline</label>
                            <input type="datetime-local" className="form-control" value={deadline} onChange={e => setDeadline(e.target.value)} id="deadline-input" required={true}/>
                        </div>
                    </div>
                </div>
                <div className="form-group">
                    <div className="input-group">
                        <Dropdown  className="btn-group">
                            <Dropdown.Toggle type="button" className="btn dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                {statuses[status]}
                            </Dropdown.Toggle>
                            <Dropdown.Menu className="dropdown-menu">
                                <Dropdown.Item className="dropdown-item" onClick={e => setStatus(0)} active={status === 0} href="#">Not started</Dropdown.Item>
                                <Dropdown.Item className="dropdown-item" onClick={e => setStatus(1)} active={status === 1} href="#">In process</Dropdown.Item>
                                <Dropdown.Item className="dropdown-item" onClick={e => setStatus(2)} active={status === 2} href="#">Completed</Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                </div>
                <div className="form-group">
                    <div className="input-group">
                        <Dropdown  className="btn-group">
                            <Dropdown.Toggle type="button" className="btn dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                {priorities[priority]}
                            </Dropdown.Toggle>
                            <Dropdown.Menu className="dropdown-menu">
                                <Dropdown.Item className="dropdown-item" onClick={e => setPriority(0)} active={priority === 0} href="#">Hidden</Dropdown.Item>
                                <Dropdown.Item className="dropdown-item" onClick={e => setPriority(1)} active={priority === 1} href="#">Standard</Dropdown.Item>
                                <Dropdown.Item className="dropdown-item" onClick={e => setPriority(2)} active={priority === 2} href="#">High</Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                </div>
                <div className="form-group">
                    <div className="input-group">
                        <div className="form-group">
                            <input className="form-check-input" type="checkbox" onChange={() => setReminded(!reminded)} checked={reminded} id="item-remind-input" />
                            <label className="form-check-label" htmlFor="item-remind-input">
                                Reminded
                            </label>
                        </div>
                    </div>
                </div>
                <Error errors={errors}/>
                <button type="submit" onClick={e => sendData(e)} id="submit" className="btn btn-primary">Submit</button>
            </form>
        )
}