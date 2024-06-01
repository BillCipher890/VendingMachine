import { useEffect, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';

function CoinAdminControl() {
    const [allCoins, getAllCoins] = useState();
    useEffect(() => {
        getAllCoinsMethod();
    }, []);

    async function getAllCoinsMethod() {
        const url = new URL('https://localhost:7101/api/Coin/AllCoin');

        try {
            const response = await fetch(url);

            if (response.ok) {
                const responseData = await response.json();
                getAllCoins(responseData);
            } else {
                console.error('Ошибка при получении монет: код ошибки ' + response.status);
            }
        } catch (error) {
            console.error('Произошла ошибка:', error);
        }
    }

    const handleAddCoins = (index) => {
        const newQuantity = prompt('Введите количество монет для добавления:');
        if (newQuantity) {
            const updatedCoins = [...allCoins];
            updatedCoins[index].count += parseInt(newQuantity);
            SetCoin(updatedCoins[index]);
            getAllCoins(updatedCoins);
        }
    };

    const handleRemoveCoins = (index) => {
        const newQuantity = prompt('Введите количество монет для удаления:');
        if (newQuantity) {
            const updatedCoins = [...allCoins];
            updatedCoins[index].count -= parseInt(newQuantity);
            SetCoin(updatedCoins[index]);
            getAllCoins(updatedCoins);
        }
    };

    const handleBlockCoin = (index) => {
        const updatedCoins = [...allCoins];
        updatedCoins[index].isBlocked = !updatedCoins[index].isBlocked;
        SetCoin(updatedCoins[index]);
        getAllCoins(updatedCoins);
    };

    async function SetCoin(coin) {
        const url = new URL('https://localhost:7101/api/Coin/UpdateCoin');

        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(coin)
            });

            if (!response.ok) {
                console.error('Ошибка при обновлении монеты: код ошибки ' + response.status);
            }
        } catch (error) {
            console.error('Произошла ошибка:', error);
        }
    }

    return (
        <table>
            <thead>
                <tr>
                    <th>Номинал</th>
                    <th>Колличество</th>
                    <th>Действия</th>
                </tr>
            </thead>
            <tbody>
                {allCoins && allCoins.map((coin, index) => (
                    <tr key={index}>
                        <td>{coin.nominalValue}</td>
                        <td>{coin.count}</td>
                        <td>
                            <button onClick={() => handleAddCoins(index)}>Добавить монеты</button>
                            <button onClick={() => handleRemoveCoins(index)}>Удалить монеты</button>
                            <button onClick={() => handleBlockCoin(index)}>{coin.isBlocked ? 'Разблокировать монету' : 'Заблокировать монету'}</button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}

let fileNew;
let fileOld;
function DrinkAdminControl() {
    const [allDrinks, getAllDrinks] = useState();
    const [showPopup, setShowPopup] = useState(false);
    const [newDrink, setNewDrink] = useState({
        name: '',
        cost: 0,
        count: 0,
        imagePath: ''
    });

    useEffect(() => {
        getAllDrinksMethod();
    }, [ ]);

    async function getAllDrinksMethod() {
        const url = new URL('https://localhost:7101/api/Drink/AllDrink');

        try {
            const response = await fetch(url);

            if (response.ok) {
                const responseData = await response.json();
                getAllDrinks(responseData);
            } else {
                console.error('Ошибка при получении напитков: код ошибки ' + response.status);
            }
        } catch (error) {
            console.error('Произошла ошибка:', error);
        }
    }

    const ImageComponent = ({ guid }) => {
        const [imageUrl, setImageUrl] = useState('');

        useEffect(() => {
            const url = new URL('https://localhost:7101/api/Drink/GetImage');
            url.searchParams.append("guid", guid);

            setImageUrl(url.toString());
        }, [guid]);

        return <img src={imageUrl} alt="Drink" className="drink-image" />;
    };

    const handleAddDrink = () => {
        setShowPopup(true);
    };

    const handleImageChangeForNew = (event) => {
        fileNew = event.target.files[0];
    };

    const handleImageChangeForOld = (event) => {
        fileOld = event.target.files[0];
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setNewDrink({ ...newDrink, [name]: value });
    };

    const handleSubmit = async () => {
        newDrink.guid = uuidv4();

        const url = new URL('https://localhost:7101/api/Drink/AddDrink');

        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newDrink)
            });

            if (response.ok) {
                // Обновление списка напитков после успешного добавления
                getAllDrinksMethod();
                setShowPopup(false);
                setNewDrink({
                    name: '',
                    cost: 0,
                    count: 0,
                    imagePath: ''
                });
            } else {
                console.error('Ошибка при добавлении напитка: код ошибки ' + response.status);
            }

            SendNewImage(newDrink.guid, fileNew);
        } catch (error) {
            console.error('Произошла ошибка:', error);
        }
    };

    function SendNewImage(guid, file) {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('guid', guid);

        fetch('https://localhost:7101/api/Drink/UploadImage', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    getAllDrinksMethod();
                    console.log('Файл успешно загружен на сервер');
                } else {
                    console.error('Ошибка загрузки файла на сервер.' + response.statusText);
                }
            });
    }

    const handleDeleteDrink = (drink) => {
        DeleteDrink(drink);
    };

    async function DeleteDrink(drink) {
        const url = new URL('https://localhost:7101/api/Drink/DeleteDrink');

        try {
            const response = await fetch(url, {
                method: 'Delete',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(drink)
            });

            if (response.ok) {
                getAllDrinksMethod();
            } else {
                console.error('Ошибка при обновлении монеты: код ошибки ' + response.status);
            }
        } catch (error) {
            console.error('Произошла ошибка:', error);
        }
    }

    const handleChangeImageDrink = (drink) => {
        ChangeImageDrink(drink);
    };

    async function ChangeImageDrink(drink) {
        SendNewImage(drink.guid, fileOld);
    }

    const handleChangeCountDrink = (index) => {
        const newCount = prompt('Введите новое количество напитков:');
        if (newCount) {
            const updatedDrinks = [...allDrinks];
            updatedDrinks[index].count = newCount;
            SetDrink(updatedDrinks[index]);
            getAllDrinks(updatedDrinks);
        }
    };

    const handleChangeCostDrink = (index) => {
        const newCost = prompt('Введите новую цену напитка:');
        if (newCost) {
            const updatedDrinks = [...allDrinks];
            updatedDrinks[index].cost = newCost;
            SetDrink(updatedDrinks[index]);
            getAllDrinks(updatedDrinks);
        }
    };

    async function SetDrink(drink) {
        const url = new URL('https://localhost:7101/api/Drink/UpdateDrink');

        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(drink)
            });

            if (!response.ok) {
                console.error('Ошибка при обновлении напитка: код ошибки ' + response.status);
            } 
        } catch (error) {
            console.error('Произошла ошибка:', error);
        }
    }

    return (
        <div>
        <table>
            <thead>
                <tr>
                    <th>Изображение</th>
                    <th>Имя</th>
                    <th>Стоймость</th>
                    <th>Колличество</th>
                    <th>Действия</th>
                </tr>
            </thead>
            <tbody>
                {allDrinks && allDrinks.map((drink, index) => (
                    <tr key={index}>
                        <td>
                            <ImageComponent guid={drink.guid} />
                            <input type="file" onChange={handleImageChangeForOld} />
                        </td>
                        <td>{drink.name}</td>
                        <td>{drink.cost}</td>
                        <td>{drink.count}</td>
                        <td>
                            <button onClick={() => handleDeleteDrink(drink)}>Удалить напиток</button>
                            <button onClick={() => handleChangeCountDrink(index)}>Изменить количество напитка</button>
                            <button onClick={() => handleChangeCostDrink(index)}>Изменить стоимость напитка</button>
                            <button onClick={() => handleChangeImageDrink(drink)}>Изменить изображение напитка</button>
                        </td>
                    </tr>
                ))}
                    <tr>
                        <td>
                            {!showPopup && (<button onClick={() => handleAddDrink()}>Добавить напиток</button>)}
                            {showPopup && (<input type="file" onChange={handleImageChangeForNew} />)}
                        </td>
                        <td>{showPopup && (<input type="text" name="name" value={newDrink.name} className="popup-drink-input" onChange={handleChange} placeholder="Имя напитка" />)}</td>
                        <td>{showPopup && (<input type="number" name="cost" value={newDrink.cost} className="popup-drink-input" onChange={handleChange} placeholder="Стоимость" />)}</td>
                        <td>{showPopup && (<input type="number" name="count" value={newDrink.count} className="popup-drink-input" onChange={handleChange} placeholder="Количество" />)}</td>
                        <td>{showPopup && (<button onClick={handleSubmit}>Добавить</button>)}</td>
                    </tr>
            </tbody>
            </table>
        </div>
    );
}

const AdminPage = 
    <div>
        <h3>Control Page</h3>
        <br />
        <CoinAdminControl />
        <br />
        <DrinkAdminControl />
    </div>;

export default AdminPage;