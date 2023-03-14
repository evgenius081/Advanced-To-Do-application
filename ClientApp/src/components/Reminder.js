import React, { useContext, useEffect, useState} from 'react'
import {TokenContext} from "../App";
import {ToastContainer} from "react-bootstrap";
import Toast from "react-bootstrap/Toast";

export function Reminder(){
    const { reminded, getReminded } = useContext(TokenContext);
    const [ showNotifications, setShowNotifications ] = useState(false)

    useEffect(() => {
        getReminded().then()
        const timer = setInterval(async () => {
            getReminded().then()
        }, 60000);
        return () => clearTimeout(timer);
    }, [getReminded]);

    return (
    <>
        <div className={showNotifications ? "show-notifications active" : "show-notifications"} onClick={() => setShowNotifications(!showNotifications)}>{showNotifications ? "Hide" : "Show"} notifications</div>
        {showNotifications ? <ToastContainer className="position-fixed">
            {reminded.length === 0 ? <h5>No notifications</h5> : ""}
            {reminded.map(item => (
                <Toast key={reminded.indexOf(item)} bg="warning" className="notification" show={item.remind}>
                    <Toast.Header>
                        <strong className="me-auto">Notification</strong>
                        <small>{parseInt((Date.parse(item.deadline) - Date.now())/60000)} minute{parseInt((Date.parse(item.deadline) - Date.now())/60000) === 1 ? "" : "s"} left</small>
                    </Toast.Header>
                    <Toast.Body>Task "{item.title}" has close deadline.</Toast.Body>
                </Toast>
            ))}
        </ToastContainer>
        : "" }
    </>
    )
}