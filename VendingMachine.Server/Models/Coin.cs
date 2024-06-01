using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Server.Models.Coin
{
    public class Coin
    {
        [Key] public uint NominalValue { get; set; }
        public uint Count { get; set; }
        public bool IsBlocked { get; set; }

        public Coin(uint nominalValue, uint count) 
        {
            NominalValue = nominalValue;
            Count = count;
            IsBlocked = false;
        }

        public Coin(uint nominalValue, uint count, bool isBlocked) : this(nominalValue, count)
        {
            IsBlocked = isBlocked;
        }

        public Coin()
        {

        }
    }
}
