using VendingMachine.Server.CountChange;
using VendingMachine.Server.Models.Coin;

namespace TestVendingMachineServer
{
    public class UnitTestCountChangeRU
    {
        [Fact]
        public void GetChange_ReturnsCorrectChange()
        {
            // Arrange
            var coins = new List<Coin>
            {
                new() { NominalValue = 10, Count = 5 },
                new() { NominalValue = 5, Count = 10 }
            };
            var countChangeRU = new CountChangeRU();

            // Act
            var result = countChangeRU.GetChange(coins, 50, 25);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(1u, result[0][0]); // количество монет номиналом 5
            Assert.Equal(5u, result[0][1]); // номинал монеты 5
            Assert.Equal(2u, result[1][0]); // количество монет номиналом 10
            Assert.Equal(10u, result[1][1]); // номинал монеты 10
        }

        [Fact]
        public void GetChange_NotEnoughCoins_ReturnsNull()
        {
            // Arrange
            var coins = new List<Coin>
            {
                new Coin { NominalValue = 10, Count = 1 },
                new Coin { NominalValue = 5, Count = 2 }
            };
            var countChangeRU = new CountChangeRU();

            // Act
            var result = countChangeRU.GetChange(coins, 50, 25);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetChange_ReturnsNull_WhenNoChangeRequired()
        {
            // Arrange
            var coins = new List<Coin>
            {
                new Coin { NominalValue = 10, Count = 5 },
                new Coin { NominalValue = 5, Count = 10 }
            };
            var countChangeRU = new CountChangeRU();

            // Act
            var result = countChangeRU.GetChange(coins, 25, 25);

            // Assert
            Assert.Null(result);
        }
    }
}