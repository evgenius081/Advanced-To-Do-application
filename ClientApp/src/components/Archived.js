import React, {useCallback, useContext, useEffect, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';

import './Home.css'
import 'font-awesome/css/font-awesome.min.css';
import {TokenContext} from "../App";
import {deleteList, updateList} from "./lists/listOperations";
import {ListElement} from "./Home";


export function Archived(){
    let navigate = useNavigate()
    const { token, getReminded } = useContext(TokenContext);

        const [lists, setLists] = useState( [
            {
                id: -1,
                title: "",
                isArchived: true,
                notStarted: 0,
                inProcess: 0,
                completed: 0
            },
        ]);

    const getLists = useCallback(async () => {
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
                    navigate("/login")
                }
                else if (response.status === 500){
                    navigate("/error")
                }
            })
    }, [setLists, navigate, token])

    async function handleDeleteList(id){
        await deleteList(id, token).then((response) => {
            if (response.ok){
                getReminded()
               setLists(lists.filter((list) => list.id !== id))
            }else if (response.status === 401){
                navigate("/login")
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
            })
    }

    async function handleDearchive(id){
        let list = lists.filter(list => list.id === id)[0]
        let data = {
            id: list.id,
            isArchived: false,
            title: list.title,
        }
        await updateList(data, token).then((response) => {
            if (response.ok){
                setLists(lists.filter((list) => list.id !== id))
            }else if (response.status === 401){
                navigate("/login")
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
