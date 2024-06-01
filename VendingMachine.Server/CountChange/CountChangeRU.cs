using VendingMachine.Server.Models.Coin;

namespace VendingMachine.Server.CountChange
{
    public class CountChangeRU : ICountChange
    {
        private uint[] coinValues;
        private uint[] coinStock;

        public uint[][] GetChange(List<Coin> coins, uint money, uint cost)
        {
            var amountToReturn = money - cost;
            if (amountToReturn == 0)
                return null;

            coinValues = coins.Select(c => c.NominalValue).Reverse().ToArray();
            coinStock = coins.Select(c => c.Count).Reverse().ToArray();

            var coinsToReturn = new uint[coinValues.Length];
            var bestCoinsToReturn = new uint[coinValues.Length];
            var bestResult = ulong.MaxValue;

            CalculateChange(0, amountToReturn, coinsToReturn, bestCoinsToReturn, ref bestResult);

            if (bestResult == ulong.MaxValue)
                return null;

            var result = bestCoinsToReturn.Zip(coinValues, (a, b) => new uint[] { a, b }).ToArray();
            return result;
        }

        private void CalculateChange(int index, long remainingAmount, uint[] coinsToReturn, uint[] bestCoinsToReturn, ref ulong bestResult)
        {
            if (remainingAmount == 0)
            {
                var totalCoins = coinsToReturn.Sum();
                if (totalCoins < bestResult)
                {
                    bestResult = totalCoins;
                    Array.Copy(coinsToReturn, bestCoinsToReturn, coinsToReturn.Length);
                }
                return;
            }

            if (index >= coinValues.Length || remainingAmount < 0)
                return;

            for (uint i = 0; i <= coinStock[index]; i++)
            {
                coinsToReturn[index] = i;
                CalculateChange(index + 1, remainingAmount - i * coinValues[index], coinsToReturn, bestCoinsToReturn, ref bestResult); 
                coinsToReturn[index] = 0;
            }
        }
    }

    public static class ArrayExtensions
    {
        public static ulong Sum(this uint[] array)
        {
            ulong result = 0;

            foreach (var num in array)
                result += num;

            return result;
        }
    }
}
