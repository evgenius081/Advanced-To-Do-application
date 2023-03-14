import { Link } from 'react-router-dom';
import React from 'react'

import "./500.css"

export function NotFound(props) {
    return (
        <section className="error">
        <div class="error-header">
            <h1>Not found</h1>
        </div>
        <div className="error-img">
            <img src="img/404.jpg" alt="astronaut" />
        </div>
        <div className="error-text">
            <p>The page you are searching was not found. Try the other one.</p>
            <p>Proceed to <Link to="/">main</Link></p>
            </div>
    </section>
    )
}