import React, {useState, useEffect, useContext, useCallback} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useParams } from 'react-router-dom';
import './View.css'
import Dropdown from "react-bootstrap/Dropdown";
import {faCheck, faEllipsisH, faEllipsisV, faEye, faStar} from "@fortawesome/fontawesome-free-solid";
import Modal from "react-bootstrap/Modal";
import { TokenContext } from "../../App";
import {faEdit, faEyeSlash, faTrashCan, faCopy } from "@fortawesome/free-regular-svg-icons";
import {deleteItem, updateItem} from "../items/itemOperations";
import {deleteList} from "./listOperations";
import Toast from 'react-bootstrap/Toast'
import { ToastContainer } from 'react-bootstrap';

const statuses = ["Not started", "In process", "Completed"]

export function ViewList(){
    let { id } = useParams();
    let navigate = useNavigate()

    const [list, setList] = useState({
        id: -1,
        title: "",
        isArchived: false
    })
    const [ showHidden, setShowHidden ] = useState(true)
    const [ showCompleted, setShowCompleted ] = useState(true)
    const [show, setShow] = useState(false);
    const [showControls, setShowControls] = useState(false)
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [items, setItems] = useState([
        {
            id: -1,
            title: "",
            remind: false,
            priority: 1,
            status: 1,
            deadline: ""
        }]);
    const { token, getReminded } = useContext(TokenContext);

    const getItems = useCallback(async () => {
        await fetch(process.env.REACT_APP_ASP_LINK+"/items/list/"+id,{
                headers: {
                    'Authorization': `Bearer ${token}`
                }})
            .then(async (response) => {
                if (response.ok) {
                    let data = await response.json()
                    setItems( data.sort((a, b) => {
                        if ((a.priority === 0 && b.priority === 0) || (a.priority !== 0 && b.priority !== 0)){
                            return 0
                        }
                        else if (a.priority === 0){
                            return 1
                        }

                        return -1
                    }))
                }else if (response.status === 401){
                    navigate("/login")
                }else if (response.status === 404){
                    navigate("/404")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }, [id, token, setItems, navigate])

    const getList = useCallback(async () => {
        await fetch(process.env.REACT_APP_ASP_LINK+"/lists/"+id,
            {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            .then(async (response) => {
                if (response.ok){
                    let data = await response.json()
                    setList(data)
                }
                else if (response.status === 401){
                    navigate("login")
                }
                else if (response.status === 404){
                    navigate("/notFound")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }, [id, token, setList, navigate])

    const [showMessage, setShowMessage] = useState(false);

useEffect(() => {
    getItems().then()
    getList().then()
}, [getItems, getList]);

    async function changeStatus(item, newStatus) {
        item.status = newStatus
        await updateItem(item, token).then((response) => {
            if (response.ok){
                getReminded()
                let new_items = items.map(i => {
                    if (i.id === item.id){
                        return item;
                    }
                    else{
                        return i;
                    }
                })
                setItems(new_items)
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    async function handleDeleteItem(id){
        await deleteItem(id, token).then((response) => {
            if (response.ok){
                getReminded()
                setItems(items.filter((list) => list.id !== id))
            }else if(response.status === 404){
                getReminded()
                setItems(items.filter((list) => list.id !== id))
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    async function handleDeleteList(id){
        await deleteList(id, token).then((response) => {
            if (response.ok){
                getReminded()
                navigate("/")
            }else if(response.status === 404){
                getReminded()
                navigate("/")
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    async function handleCopy(){
        await fetch(process.env.REACT_APP_ASP_LINK+"/lists/copy/"+id, {
            headers: {
                'Authorization': `Bearer ${token}`
            },
        })
            .then(async (response) => {
                if (response.ok){
                    getReminded()
                    setShowMessage(true)
                }
                else if (response.status === 404){
                    navigate("/notFound")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }

    async function changeRemind(item){
        item.remind = !item.remind
        await updateItem(item, token)
            .then(async (response) => {
                if (response.ok){
                    getReminded()
                    let new_items = items.map(i => {
                        if (i.id === item.id){
                            return item;
                        }
                        else{
                            return i;
                        }
                    })
                    setItems(new_items)
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }

    async function changeHidden(item){
        item.priority = item.priority === 0 ? 1 : 0
        await updateItem(item, token).then((response) => {
            if (response.ok){
                let new_items = items.map(i => {
                    if (i.id === item.id){
                        return item;
                    }
                    else{
                        return i;
                    }
                })
                setItems(new_items)
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    
    async function changePriority(item){
        item.priority = item.priority === 2 ? 1 : 2
        await updateItem(item, token).then((response) => {
            if (response.ok){
                let new_items = items.map(i => {
                    if (i.id === item.id){
                        return item;
                    }
                    else{
                        return i;
                    }
                })
                setItems(new_items)
            }
            else if (response.status === 500){
                navigate("/error")
            }
        })
    }

    let hidden = showCompleted ? items.filter(item => item.priority === 0) : items.filter(item => item.status !== 2 && item.priority === 0)
    let notHidden = showCompleted ? items.filter(item => item.priority !== 0) : items.filter(item => item.status !== 2 && item.priority !== 0)
    return (
        <>
            <section className="view-list-header">
                <div>
                    <h2>{list.title} {list.isArchived ? <><br/><span>Archived</span></> : ""}</h2>
                    { items.length !== 0 && items[0].id === -1 ? "" :
                        <>
                    <Link id="edit-list" to={"/todos/edit/"+id}><FontAwesomeIcon size="2x" icon={faEdit} /></Link>
                    <div id="delete-list" onClick={handleShow}><FontAwesomeIcon size="2x" icon={faTrashCan} /></div>
                    <div id="copy-list" onClick={handleCopy}><FontAwesomeIcon size="2x" icon={faCopy} /></div>
                    <Modal show={show} onHide={handleClose}>
                        <Modal.Header closeButton>
                            <Modal.Title>Delete list</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>Are you sure you want to delete this list? This operation cannot be undone.</Modal.Body>
                        <Modal.Footer>
                            <button className="close-deleting-list" onClick={handleClose}>
                                Close
                            </button>
                            <button className="accept-deleting-list" onClick={() => handleDeleteList(id)}>
                                Delete
                            </button>
                        </Modal.Footer>
                    </Modal>
                    <ToastContainer position="top-end" className="position-fixed">
                        <Toast bg="success" delay={2000} onClose={() => setShowMessage(false)} show={showMessage} autohide>
                            <Toast.Body>Copied</Toast.Body>
                        </Toast>
                    </ToastContainer>
                        </>
                    }
                </div>
            </section>
            <section id="items">
                {notHidden.length === 0 && hidden.length === 0 ? <h4>No items here.</h4> : ""}
                {notHidden.map(item => (
                    <ItemElement item={item} changeHidden={changeHidden} changePriority={changePriority} changeRemind={changeRemind} deleteItem={deleteItem} handleDelete={handleDeleteItem} changeStatus={changeStatus} todo={id} key={items.indexOf(item)}/>
                ))}
                {(showHidden && (hidden.length !== 0)) ?
                    <div className="separator">
                        <hr/>
                        <p>Hidden</p>
                        <hr />
                    </div>
                    : ""}
                {showHidden ? hidden.map(item => (
                    <ItemElement item={item} changeHidden={changeHidden} changePriority={changePriority} changeRemind={changeRemind} handleDelete={handleDeleteItem} changeStatus={changeStatus} todo={id} key={items.indexOf(item)}/>
                )) : ""}
            </section>
            { items.length !== 0 && items[0].id === -1 ? "" :
                <section className="view-controls" style={showControls ? {width: "230px"} : {}}>
                    {showControls ?
                        <>
                    <div id="show-completed" onClick={() => setShowCompleted(!showCompleted)}>
                        <p>{showCompleted ? "Hide" : "Show"} completed</p>
                        <p>{showCompleted ? <span></span> : ""}<FontAwesomeIcon style={showCompleted ? {marginRight: "3px"} : {}} icon={ faCheck } /></p>
                    </div>
                    <div id="show-hidden" onClick={() => setShowHidden(!showHidden)}>
                        <p>{showHidden ? "Hide" : "Show"} hidden</p>
                        <p><FontAwesomeIcon icon={ showHidden ? faEyeSlash : faEye } /></p>
                    </div>
                    <div className="add-item">
                        <Link id="add" to={"/todos/"+id+"/items/create"}>
                            <p>New item</p>
                            <p>+</p>
                        </Link>
                    </div>
                        </>
                        : "" }
                    <div className="show-view-controls" onClick={() => setShowControls(!showControls)}>
                        <FontAwesomeIcon icon={ showControls ? faEllipsisH : faEllipsisV } size="2x" />
                    </div>
                </section>
            }
        </>
    )
}

export function ItemElement(props) {
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
        const item = props.item
        return (
            <article>
            <div className="item-title">
                <Link to={"/todos/"+props.todo+"/items/"+item.id}>{item.title}</Link>
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
                            props.handleDelete(item.id)
                            handleClose()
                        }}>
                            Delete
                        </button>
                    </Modal.Footer>
                </Modal>
            <div className="item-status">
            <div className='item-starred' onClick={() => props.changePriority(item)}>
                <FontAwesomeIcon style={item.priority === 2 ? {color: "#fdd835"} : {}} icon={ faStar} />
            </div>
                <Dropdown  className="btn-group">
                    <Dropdown.Toggle type="button" className="btn dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <span className={item.status === 2 ? "completed-status-comp" : item.status === 1 ? "completed-status-proc" : "completed-status-not-st"}>
                            {statuses[item.status]}
                        </span>
                    </Dropdown.Toggle>
                    <Dropdown.Menu className="dropdown-menu">
                        <Dropdown.Item className="dropdown-item" onClick={e => props.changeStatus(item, 0)}>
                            <span className="completed-status-not-st">
                                Not started
                            </span>
                        </Dropdown.Item>
                        <Dropdown.Item className="dropdown-item" onClick={e => props.changeStatus(item, 1)}>
                            <span className="completed-status-proc">
                                In process
                            </span>
                        </Dropdown.Item>
                        <Dropdown.Item className="dropdown-item" onClick={e => props.changeStatus(item, 2)}>
                            <span className="completed-status-comp">
                                Completed
                            </span>
                        </Dropdown.Item>
                    </Dropdown.Menu>
                </Dropdown>
                {item.id === -1 ? <div style={{marginRight: "0.75rem"}}></div> :<div className="item-controls">
                    <Dropdown  className="btn-group" drop="end">
                        <Dropdown.Toggle type="button" className="btn dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <FontAwesomeIcon icon={ faEllipsisV } />
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu" style={{transition: "translate(92px, 0px)"}}>
                            <Dropdown.ItemText className="dropdown-item">
                                <Link tag={Link} className="edit-item" to={"/todos/"+props.todo+"/items/edit/"+item.id}>Edit</Link>
                            </Dropdown.ItemText>
                            <Dropdown.Toggle className="dropdown-item">
                                <div className="show-item" onClick={() => props.changeHidden(item)}>{item.priority === 0 ? "Show" : "Hide"}</div>
                            </Dropdown.Toggle>
                            <Dropdown.Toggle className="dropdown-item">
                                <div className="remind-item" onClick={() => props.changeRemind(item)}>{!item.remind ? "Remind" : "No remind"}</div>
                            </Dropdown.Toggle>
                            <Dropdown.ItemText className="dropdown-item">
                                <div className="delete-item" onClick={handleShow}>Delete</div>
                            </Dropdown.ItemText>
                        </Dropdown.Menu>
                    </Dropdown>
                </div>}
            </div>
        </article>
        )

}