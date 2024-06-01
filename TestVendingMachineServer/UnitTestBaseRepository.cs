using Microsoft.EntityFrameworkCore;
using VendingMachine.Server.Models.Coin;
using VendingMachine.Server.Models;
using VendingMachine.Server.Repositories;
using VendingMachine.Server;
using Microsoft.AspNetCore.Routing;

namespace TestVendingMachineServer
{
    public class UnitTestBaseRepository
    {
        [Fact]
        public async Task AddAsync_Coin_AddsCoinToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddAsync_Coin_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Coin>(context);
                var coin = new Coin(20, 10);

                // Act
                await repository.AddAsync(coin);

                // Assert
                Assert.Equal(coin, await context.Coins.FindAsync(coin.NominalValue));
            }
        }

        [Fact]
        public async Task AddAsync_Drink_AddsDrinkToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddAsync_Drink_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Drink>(context);
                var drink = new Drink("Cola", 100, 5, "cola.jpg");

                // Act
                await repository.AddAsync(drink);

                // Assert
                Assert.Equal(drink, await context.Drinks.FindAsync(drink.Guid));
            }
        }

        [Fact]
        public async Task AddListAsync_Coin_AddsListOfCoinsToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddListAsync_Coin_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Coin>(context);
                var coins = new List<Coin> { new Coin(20, 10), new Coin(30, 20) };

                // Act
                await repository.AddListAsync(coins);

                // Assert
                Assert.Equal(coins[0], await context.Coins.FindAsync(coins[0].NominalValue));
                Assert.Equal(coins[1], await context.Coins.FindAsync(coins[1].NominalValue));
            }
        }

        [Fact]
        public async Task AddListAsync_Drink_AddsListOfDrinksToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddListAsync_Drink_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Drink>(context);
                var drinks = new List<Drink> { new Drink("Cola", 100, 5, "cola.jpg"), new Drink("Water", 50, 10, "water.jpg") };

                // Act
                await repository.AddListAsync(drinks);

                // Assert
                Assert.Equal(drinks[0], await context.Drinks.FindAsync(drinks[0].Guid));
                Assert.Equal(drinks[1], await context.Drinks.FindAsync(drinks[1].Guid));
            }
        }

        [Fact]
        public async Task DeleteAsync_Coin_DeletesCoinFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_Coin_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Coin>(context);
                var coin = new Coin(20, 10);
                var countBeforeAdd = await context.Coins.CountAsync();
                await repository.AddAsync(coin);

                // Act
                await repository.DeleteAsync(coin);

                // Assert
                Assert.Equal(countBeforeAdd, await context.Coins.CountAsync());
            }
        }

        [Fact]
        public async Task DeleteAsync_Drink_DeletesDrinkFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_Drink_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Drink>(context);
                var drink = new Drink("Cola", 100, 5, "cola.jpg");
                var countBeforeAdd = await context.Drinks.CountAsync();
                await repository.AddAsync(drink);

                // Act
                await repository.DeleteAsync(drink);

                // Assert
                Assert.Equal(countBeforeAdd, await context.Drinks.CountAsync());
            }
        }

        [Fact]
        public async Task GetAllAsync_Coin_ReturnsAllCoinsFromDatabase()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetAllAsync_Coin_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Coin>(context);
                var countBeforeAdd = await context.Coins.CountAsync();

                await repository.AddAsync(new Coin(20, 10));
                await repository.AddAsync(new Coin(30, 20));

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(countBeforeAdd + 2, result.Count);
            }
        }

        [Fact]
        public async Task GetAllAsync_Drink_ReturnsAllDrinksFromDatabase()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetAllAsync_Drink_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Drink>(context);
                var countBeforeAdd = await context.Drinks.CountAsync();

                await repository.AddAsync(new Drink("Cola", 100, 5, "cola.jpg"));
                await repository.AddAsync(new Drink("WaterGas", 50, 10, "waterGas.jpg"));

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(countBeforeAdd + 2, result.Count);
            }
        }

        [Fact]
        public async Task UpdateAsync_Coin_UpdatesCoinInDatabase()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAsync_Coin_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Coin>(context);
                var coin = new Coin(20, 10);
                await repository.AddAsync(coin);

                // Update coin
                coin.Count = 15;

                // Act
                await repository.UpdateAsync(coin);

                // Get updated coin
                var updatedCoin = await context.Coins.FindAsync(coin.NominalValue);

                // Assert
                Assert.Equal(15u, updatedCoin.Count);
            }
        }

        [Fact]
        public async Task UpdateAsync_Drink_UpdatesDrinkInDatabase()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAsync_Drink_Database")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var repository = new BaseRepository<Drink>(context);
                var drink = new Drink("Cola", 100, 5, "cola.jpg");
                await repository.AddAsync(drink);

                // Update drink
                drink.Count = 8;

                // Act
                await repository.UpdateAsync(drink);

                // Get updated drink
                var updatedDrink = await context.Drinks.FindAsync(drink.Guid);

                // Assert
                Assert.Equal(8u, updatedDrink.Count);
            }
        }
    }
}
