import React, {useCallback, useState} from 'react';
import { Route, Routes} from "react-router-dom";
import { Home } from './components/Home';
import { NotFound } from './components/404'
import { CreateItem } from './components/items/Create';
import { CreateList } from './components/lists/Create';
import { EditItem } from './components/items/Edit';
import { EditList } from './components/lists/Edit';
import { ViewItem } from './components/items/View';
import { ViewList } from './components/lists/View';
import { Login } from "./components/Login";
import {NavMenu} from "./components/NavMenu";
import { Archived } from "./components/Archived";
import {Footer} from "./components/Footer";
import {Today} from "./components/Today";
import {Reminder} from "./components/Reminder";
import { Priority } from './components/Priority';
import { InternalError } from './components/500';

export const TokenContext = React.createContext("");

export function App(){
    //                                 I'll burn in hell for that, sorry :(
    const [token, setToken] = useState(sessionStorage.getItem("todoJWT"))
    const [username, setUsername] = useState("")
    const [reminded, setReminded] = useState([])

    const getReminded = useCallback(async () => {
        await fetch(process.env.REACT_APP_ASP_LINK+"/items/remind", {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        }).then(async (response) => {
            if (response.ok){
                let data = await response.json()
                setReminded(data)
            }
        })
    }, [token])

        return (
            <>
                    <TokenContext.Provider value={{ getReminded: getReminded, token: token, setToken: setToken, username: username, setUsername: setUsername, reminded: reminded, setReminded: setReminded}}>
                        <Reminder />
                        <NavMenu/>
                        <main>
                            <Routes>
                                <Route exact path='/' element={<Home/>} />
                                <Route exact path='/todos/' element={<Home/>} />
                                <Route exact path='/todos/archived' element={<Archived />}/>
                                <Route exact path='/items/today' element={<Today />}/>
                                <Route exact path='/items/priority' element={<Priority />}/>
                                <Route exact path='/login' element={<Login />} />
                                <Route exact path='/todos/:id' element={<ViewList/>} />
                                <Route exact path='/todos/edit/:id' element={<EditList />} />
                                <Route exact path='/todos/create' element={<CreateList />}/>
                                <Route exact path='/todos/:list_id/items/:item_id' element={<ViewItem />} />
                                <Route exact path='/todos/:list_id/items/edit/:item_id' element={<EditItem />} />
                                <Route exact path='/todos/:list_id/items/create' element={<CreateItem />}/>
                                <Route exact path='/error' element={<InternalError />}/>
                                <Route path='*' element={<NotFound />} />
                            </Routes>
                        </main>
                        <Footer />
                    </TokenContext.Provider>
            </>
        );

}