import { useEffect, useState } from 'react';


function CommonClientControl() {
    const [money, setMoney] = useState(0);
    const [change, SetChange] = useState();

    function DrinkClientControl() {
        const [allDrinks, getAllDrinks] = useState();

        useEffect(() => {
            getAllDrinksMethod();
        }, []);

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

        const handleGetDrink = (index) => {
            GetDrink(index);
        };

        function GetDrink(index) {
            const updatedDrinks = [...allDrinks];
            updatedDrinks[index].count -= 1;

            getAllDrinks(updatedDrinks);
            GetChange(updatedDrinks[index]);
        }

        async function GetChange(drink) {
            const url = new URL('https://localhost:7101/api/Drink/ClientGetDrink');
            const data = { money: money, drink: drink };

            try {
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    const responseData = await response.text();
                    setMoney(0);
                    SetChange(responseData);
                }
                else {
                    console.error('Ошибка при обновлении напитка: код ошибки ' + response.status);
                }
            } catch (error) {
                console.error('Произошла ошибка:', error);
            }
        }

        function GetIsBlocked(index) {
            const updatedDrinks = [...allDrinks];
            return money - updatedDrinks[index].cost < 0;
        }

        return (
            <div>
                <table>
                    <thead>
                        <tr>
                            <th>Изображение</th>
                            <th>Имя</th>
                            <th>Стоймость</th>
                            <th>Осталось</th>
                            <th>Действия</th>
                        </tr>
                    </thead>
                    <tbody>
                        {allDrinks && allDrinks.map((drink, index) => (
                            drink.count !== 0 && (
                                <tr key={index}>
                                    <td>
                                        <ImageComponent guid={drink.guid} />
                                    </td>
                                    <td>{drink.name}</td>
                                    <td>{drink.cost}</td>
                                    <td>{drink.count}</td>
                                    <td>
                                        <button onClick={() => handleGetDrink(index)} disabled={GetIsBlocked(index)}>Получить</button>
                                    </td>
                                </tr>
                            )
                        ))}
                    </tbody>
                </table>
                <br />
                <div>
                    <h3>{change}</h3>
                </div>
            </div>
        );
    }

    function CoinClientControl() {
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

        const handleAddCoin = (index) => {
            AddCoin(index);
        };

        async function AddCoin(index) {
            const updatedCoins = [...allCoins];
            const newMoney = money + updatedCoins[index].nominalValue;
            setMoney(newMoney);

            updatedCoins[index].count += 1;
            SetCoin(updatedCoins[index]);
        }

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

        function GetIsBlocked(nominal) {
            if (!allCoins)
                return false;

            for (var coin of allCoins) {
                if (coin.nominalValue === nominal) {
                    return coin.isBlocked;
                }
            }
            return false;
        }

        return (
            <div>
                <div>
                    <h3>{money}</h3>
                </div>
                <table>
                    <thead>
                        <tr>
                            <th>1</th>
                            <th>2</th>
                            <th>5</th>
                            <th>10</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td><button onClick={() => handleAddCoin(0)} disabled={GetIsBlocked(1)}>Добавить монету</button></td>
                            <td><button onClick={() => handleAddCoin(1)} disabled={GetIsBlocked(2)}>Добавить монету</button></td>
                            <td><button onClick={() => handleAddCoin(2)} disabled={GetIsBlocked(5)}>Добавить монету</button></td>
                            <td><button onClick={() => handleAddCoin(3)} disabled={GetIsBlocked(10)}>Добавить монету</button></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        );
    }

    return (
        <div>
            <h3>Client Page</h3>
            <br />
            <DrinkClientControl />
            <br />
            <CoinClientControl />
        </div>
    );
}

const Client = <CommonClientControl />;

export default Client;