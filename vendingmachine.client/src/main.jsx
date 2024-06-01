import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'

const urlParams = new URLSearchParams(window.location.search);
const passwordFromUrl = urlParams.get('password');

if (passwordFromUrl != null) {
    ReactDOM.createRoot(document.getElementById('root')).render(
        <React.StrictMode>
            <App password={passwordFromUrl} />
        </React.StrictMode>
    )
}
else
{
    ReactDOM.createRoot(document.getElementById('root')).render(
        <React.StrictMode>
            <App password={ 1} />
        </React.StrictMode>
    )
}

