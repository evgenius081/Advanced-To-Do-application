import React, {useCallback, useContext, useEffect, useState} from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit , faTrashCan} from "@fortawesome/free-regular-svg-icons";
import { useParams } from 'react-router-dom';
import { convertDate } from "../../dateConverter";

import './View.css'
import {TokenContext} from "../../App";
import {deleteItem, readItem} from "./itemOperations";
import Modal from "react-bootstrap/Modal";
import refresh from "../../tokenRefresh";
import {readList} from "../lists/listOperations";

export function ViewItem (){
    let { list_id, item_id } = useParams();
    let navigate = useNavigate()
    const { getReminded } = useContext(TokenContext);
    const priorities = ["Hidden", "Standard", "High"]
    const statuses = ["Not started", "In process", "Completed"];
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [list, setList] = useState({
        id: -1,
        title: "",
        isArchived: false,
    })
    const [item, setItem] = useState({
                id: 1,
                title: "<Pending>",
                description: "",
                status: 0,
                priority: 1,
                deadline: "",
                createdAt: ""
            })

    const getItem = useCallback(async () => {
        
            await readItem(item_id).then(async (response) => {
                if (response.ok) {
                    let data = await response.json()
                    if (parseInt(list_id) !== data.toDoListID){
                        navigate("/notFound")
                    }
                    setItem(data)
                }else if (response.status === 401){
                    if (await refresh()) {
                        await getItem()
                    }else{
                        navigate("/login")
                    }
                } else if (response.status === 404){
                    navigate("/notFound")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }, [item_id, navigate, list_id])

    const getList = useCallback(async () => {
        
        await readList(list_id)
            .then(async (response) => {
                if (response.ok) {
                    let data = await response.json()
                    setList(data)
                }else if (response.status === 401){
                    if (await refresh()) {
                        await getList()
                    }else{
                        navigate("/login")
                    }
                } else if (response.status === 404){
                    navigate("/notFound")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }, [list_id, setList, navigate])

    async function handleDelete(id){

        await deleteItem(id).then(async (response) => {
            if (response.ok || response.status === 404){
                getReminded()
                navigate(-1)
            }
            else if (response.status === 500){
                navigate("/error")
            }
            else if (response.status === 401){
                if (await refresh()) {
                    await handleDelete(id)
                }else{
                    navigate("/login")
                }
            }
        })
    }

    useEffect(() =>{
        getList().then()
        getItem().then()
    }, [getItem, getList])

        return (
            <section>
                <div id="item-title">
                    <h2>{item.title}</h2>
                    <Link id="edit-item" to={"/todos/"+list_id+"/items/edit/"+item_id}><FontAwesomeIcon size="2x" icon={faEdit} /></Link>
                    <div id="delete-item" onClick={handleShow}><FontAwesomeIcon size="2x" icon={faTrashCan} /></div>
                    <Modal show={show} onHide={handleClose}>
                        <Modal.Header closeButton>
                            <Modal.Title>Delete list</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>Are you sure you want to delete this item? This operation cannot be undone.</Modal.Body>
                        <Modal.Footer>
                            <button className="close-deleting-list" onClick={handleClose}>
                                Close
                            </button>
                            <button className="accept-deleting-list" onClick={() => handleDelete(item.id)}>
                                Delete
                            </button>
                        </Modal.Footer>
                    </Modal>
                </div>
                <div id="item-description">
                    <p>{item.description}</p>
                </div>
                <div id="item-created-container">
                    <p>Created at: <b id="item-created">{convertDate(item.createdAt)}</b></p>
                </div>
                <div id="item-deadline-container">
                    <p>Due to: <b id="item-deadline">{convertDate(item.deadline)}</b></p>
                </div>
                <div id="item-status-container">
                    <p>Status: <b id="item-status">{statuses[item.status]}</b></p>
                </div>
                <div id="item-priority-container">
                    <p>Status: <b id="item-status">{priorities[item.priority]}</b></p>
                </div>
                <div id="item-reminded-container">
                    <p>Reminded: <b id="item-hidden">{item.remind ? "yes" : "no"}</b></p>
                </div>
                <div id="item-folder-container">
                    <p>Folder: <Link to={"/todos/"+list_id}>{list.title}</Link></p>
                </div>
            </section>
        )
}
