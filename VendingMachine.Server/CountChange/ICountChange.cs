using VendingMachine.Server.Models.Coin;

namespace VendingMachine.Server.CountChange
{
    public interface ICountChange
    {
        public uint[][] GetChange(List<Coin> coins, uint money, uint cost);
    }
}
