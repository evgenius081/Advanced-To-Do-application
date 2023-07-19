import React, {useState, useEffect, useContext, useCallback} from 'react';
import { useNavigate} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useParams } from 'react-router-dom';
import './lists/View.css'
import './Today.css'
import {faCheck, faEllipsisH, faEllipsisV, faEye} from "@fortawesome/fontawesome-free-solid";
import { TokenContext } from "../App";
import {faEyeSlash} from "@fortawesome/free-regular-svg-icons";
import {deleteItem, updateItem} from "./items/itemOperations";

import { ItemElement} from "./lists/View";

export function Today(){
    let { id } = useParams();
    let navigate = useNavigate()

const [ showHidden, setShowHidden ] = useState(true)
    const [ showCompleted, setShowCompleted ] = useState(true)
    const [showControls, setShowControls] = useState(false)
    const [items, setItems] = useState([
        {
            id: -1,
            title: "",
            priority: 1,
            status: 0,
            deadline: ""
        }]);
    const { token, getReminded } = useContext(TokenContext);

    const getItems = useCallback(async () => {
        await fetch(process.env.REACT_APP_ASP_LINK+"/items/today",{
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
    }, [token, setItems, navigate])

useEffect(() => {
    getItems().then()
}, [getItems]);

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

    async function handleDelete(id){
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

    let hidden = showCompleted ? items.filter(item => item.priority === 0) : items.filter(item => item.status !== 2 && item.priority === 0)

    return (
        <>
            <section className="view-today-header">
                <h2>Today</h2>
            </section>
            <section id="items">
                {items.filter(item => (item.status !== 2 || (item.status === 2 && showCompleted)) && item.priority !== 0).map(item => (
                    <ItemElement item={item} handleDelete={handleDelete} changePriority={changePriority} changeRemind={changeRemind} changeHidden={changeHidden} changeChecked={changeStatus} todo={item.toDoListID} key={items.indexOf(item)}/>
                ))}
                {(showHidden && (hidden.length !== 0)) ?
                    <div className="separator">
                        <hr/>
                        <p>Hidden</p>
                        <hr />
                    </div>
                    : ""}
                {showHidden ? hidden.map(item => (
                    <ItemElement item={item} changeHidden={changeHidden} changePriority={changePriority} changeRemind={changeRemind} handleDelete={handleDelete} changeChecked={changeStatus} todo={id} key={items.indexOf(item)}/>
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