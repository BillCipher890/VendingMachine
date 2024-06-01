import { useEffect, useState } from 'react';
import Client from './Client';
import Admin from './Admin';
import './App.css';

function App(password) {
    const [isAdmin, checkIsAdmin] = useState();

    useEffect(() => {
        getIsAdmin();
    }, []);

    const contentToBuild = isAdmin ? Admin : Client;

    return (
        <div>
            {contentToBuild}
        </div>
    );

    async function getIsAdmin() {
        const url = new URL('https://localhost:7101/api/Home/CheckPassword'); 
        url.searchParams.append("password", password.password);

        try {
            const response = await fetch(url);

            if (response.ok) {
                const responseData = await response.json();
                checkIsAdmin(responseData);
            } else {
                console.error('Ошибка при проверке пароля');
            }
        } catch (error) {
            console.error('Произошла ошибка:', error);
        }
    }
}

export default App;