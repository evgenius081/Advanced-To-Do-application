import React, {useCallback, useContext, useEffect, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';

import './Home.css'
import 'font-awesome/css/font-awesome.min.css';
import {TokenContext} from "../App";
import {deleteList, updateList} from "./lists/listOperations";
import {ListElement} from "./Home";
import jwt_decode from "jwt-decode";
import refresh from "../tokenRefresh";


export function Archived(){
    let navigate = useNavigate()
    const { getReminded } = useContext(TokenContext);
        const [lists, setLists] = useState( [
            {
                id: -1,
                title: "",
                isArchived: true,
                notStarted: 0,
                inProcess: 0,
                completed: 0,
                userID: -1
            },
        ]);

    const getLists = useCallback(async () => {
        let token = sessionStorage.getItem("todoJWT");
        await fetch(process.env.REACT_APP_ASP_LINK+"/lists/archived",
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
                    let newList = await response.json()
                    getReminded()
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

    async function handleDearchive(id){
        let token = sessionStorage.getItem("todoJWT");
        let list = lists.filter(list => list.id === id)[0]
        let data = {
            id: list.id,
            isArchived: false,
            title: list.title,
            userID: parseInt(jwt_decode(token).nameid)
        }
        await updateList(data).then(async (response) => {
            if (response.ok){
                setLists(lists.filter((list) => list.id !== id))
            }else if (response.status === 401){
                if (await refresh()){
                    await handleDearchive(id)
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
                    <h2>Archived lists</h2>
                    {(lists.length === 0 || lists[0].id === -1) ? "" : <div className="add-list"><Link id="add" to={"/todos/create"}><p>New list</p><p>+</p></Link></div>}
                </section>
                {(lists.length === 0 || lists[0].id === -1) ? "" : <div className="completed-text">
                    <span className="completed-status-comp">Completed</span>
                    <p>&nbsp;/&nbsp;</p>
                    <span className="completed-status-proc">In process</span>
                    <p>&nbsp;/&nbsp;</p>
                    <span className="completed-status-not-st">Not started</span>
                </div>}
                <section className="lists-container">
                    <div id="archived-todo-lists-list">
                        {lists.length === 0 ? <h4>There no archived items here.</h4> : lists.map(list => (
                            <ListElement list={list} deleteList={handleDeleteList} handleCopy={handleCopy} handleDearchive={handleDearchive} key={lists.indexOf(list)}/>
                        ))}
                    </div>
                </section>
            </>
        )
    }
