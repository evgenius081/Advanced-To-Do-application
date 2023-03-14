import React from 'react';

export function Error(props) {
    return (
        <section className="errors">
        {props.errors.map(error => (
                <article className="error" key={props.errors.indexOf(error)}>
                    {error}
                </article>
            ))}
        </section>
    )
}