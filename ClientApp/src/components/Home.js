import React, {useCallback, useContext, useEffect, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { library } from "@fortawesome/fontawesome-svg-core";
import { faEdit } from "@fortawesome/free-regular-svg-icons";
import { faEllipsisV } from "@fortawesome/fontawesome-free-solid"
import './Home.css'
import 'font-awesome/css/font-awesome.min.css';
import Dropdown from "react-bootstrap/Dropdown";
import Modal from 'react-bootstrap/Modal';
import {TokenContext} from "../App";
import {deleteList} from "./lists/listOperations";
import jwt_decode from "jwt-decode";
import refresh from "../tokenRefresh";

library.add(faEdit);


export function Home(){
    let navigate = useNavigate()
    const { getReminded } = useContext(TokenContext);
    const [lists, setLists] = useState( [
        {
            id: -1,
            title: "",
            isArchived: false,
            itemsNotStarted: 0,
            itemsInProcess: 0,
            itemsCompleted: 0
        },
    ]);

    const getLists = useCallback(async () => {
        let token = sessionStorage.getItem("todoJWT");
        await fetch(process.env.REACT_APP_ASP_LINK+"/lists/unarchived",
            {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            .then(async (response) => {
                if (response.ok){
                    setLists(await response.json())
                }
                else if (response.status === 401){
                    if (await refresh()){
                        await getLists()
                    }else{
                        navigate("/login")
                    }
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }, [setLists, navigate])

    async function handleDeleteList(id){
        
        await deleteList(id).then(async (response) => {
            if (response.ok){
                getReminded()
                setLists(lists.filter((list) => list.id !== id))
            }else if (response.status === 401){
                if (await refresh()){
                    await handleDeleteList(id)
                }else{
                    navigate("/login")
                }
            }
            else if(response.status === 404){
                setLists(lists.filter((list) => list.id !== id))
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    async function handleCopy(id){
        let token = sessionStorage.getItem("todoJWT");
        await fetch(process.env.REACT_APP_ASP_LINK+"/lists/copy/"+id, {
            headers: {
                'Authorization': `Bearer ${token}`
            },
        })
            .then(async (response) => {
                if (response.ok){
                    getReminded()
                    let newList = await response.json()
                    setLists([...lists, newList])
                }
                else if (response.status === 404){
                    navigate("/notFound")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
                else if (response.status === 401){
                    if (await refresh()){
                        await handleCopy(id)
                    }else{
                        navigate("/login")
                    }
                }
            })
    }

    async function handleArchive(id){
        let token = sessionStorage.getItem("todoJWT");
        let list = lists.filter(list => list.id === id)[0]
        let data = {
            id: list.id,
            isArchived: true,
            title: list.title,
            userID: parseInt(jwt_decode(token).nameid)
        }
        await fetch(process.env.REACT_APP_ASP_LINK+"/lists", {
            method: "PUT",
            headers: {
                'Authorization': `Bearer ${token}`,
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        }).then(async (response) => {
            if (response.ok){
                setLists(lists.filter((list) => list.id !== id))
            }else if (response.status === 401){
                if (await refresh()){
                    await handleArchive(id)
                }else{
                    navigate("/login")
                }
            }
            else if(response.status === 404){
                setLists(lists.filter((list) => list.id !== id))
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    useEffect(() => {
        getLists().then();
    }, [getLists])

    return (
        <>
            <section className="view-header">
                <h2>All lists</h2>
                {(lists.length === 0 || lists[0].id === -1) ? "" :
                    <div className="add-list"><Link id="add" to={"/todos/create"}><p>New list</p><p>+</p></Link></div>
                }
            </section>
            {(lists.length === 0 || lists[0].id === -1) ? "" : <div className="completed-text">
                <span className="completed-status-comp">Completed</span>
                <p>&nbsp;/&nbsp;</p>
                <span className="completed-status-proc">In process</span>
                <p>&nbsp;/&nbsp;</p>
                <span className="completed-status-not-st">Not started</span>
            </div>}
            <section className="lists-container">
                <div id="todo-lists-list">
                    {lists.length === 0 ? <h4>There are no items here.</h4> :lists.map(list => (
                        <ListElement list={list} deleteList={handleDeleteList} handleCopy={handleCopy} handleArchive={handleArchive} key={lists.indexOf(list)}/>
                    ))}
                </div>
            </section>
        </>
    )
}

export function ListElement(props){
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const list = props.list
    let all = (list.itemsInProcess + list.itemsCompleted + list.itemsNotStarted)
    return (
        <article>
            <div className="list-title">
                <Link to={"/todos/"+list.id}>{list.title}</Link>
            </div>
            <div className="completed align-middle">
                <div className="completed-bar">
                    <div className="proc-completed-bar" style={{"width": (list.itemsInProcess + list.itemsCompleted) * 100 /(all === 0 ? 1 : all) + "%" }}></div>
                    <div className="comp-completed-bar" style={{"width": (list.itemsCompleted * 100)/(all === 0 ? 1 : all) + "%" }}></div>
                </div>
                <Modal show={show} onHide={handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>Delete list</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>Are you sure you want to delete this list? This operation cannot be undone.</Modal.Body>
                    <Modal.Footer>
                        <button className="close-deleting-list" onClick={handleClose}>
                            Close
                        </button>
                        <button className="accept-deleting-list" onClick={() => {
                            props.deleteList(list.id)
                            handleClose()
                        }}>
                            Delete
                        </button>
                    </Modal.Footer>
                </Modal>
                {list.id === -1 ? <div style={{marginRight: "0.75rem"}}></div> :
                    <div className="list-controls" style={{transition: "translate(0px, 0px)"}}>
                        <Dropdown  className="btn-group" drop="end">
                            <Dropdown.Toggle type="button" className="btn dropdown-toggle" lists-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <FontAwesomeIcon icon={ faEllipsisV } />
                            </Dropdown.Toggle>
                            <Dropdown.Menu className="dropdown-menu">
                                <Dropdown.ItemText className="dropdown-item">
                                    <Link tag={Link} className="edit-list" to={"/todos/edit/"+list.id}>Edit</Link>
                                </Dropdown.ItemText>
                                <Dropdown.Toggle className="dropdown-item">
                                    <div className="archive" onClick={() => list.isArchived ? props.handleDearchive(list.id) : props.handleArchive(list.id)}>{list.isArchived ? "Dearchive" : "Archive"}</div>
                                </Dropdown.Toggle>
                                <Dropdown.Toggle className="dropdown-item">
                                    <div className="copy" onClick={() => props.handleCopy(list.id)}>Copy</div>
                                </Dropdown.Toggle>
                                <Dropdown.ItemText className="dropdown-item">
                                    <div className="delete-list" onClick={handleShow}>Delete</div>
                                </Dropdown.ItemText>
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>}
            </div>
        </article>
    )

}