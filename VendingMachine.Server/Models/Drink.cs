using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Server.Models
{
    public class Drink
    {
        [Key] public string Guid { get; set; }
        public string Name { get; set; }
        public uint Cost { get; set; }
        public uint Count { get; set; }
        public string ImagePath { get; set; }

        public Drink(string name, uint cost, uint count, string imagePath)
        {
            Guid = System.Guid.NewGuid().ToString();
            Name = name;
            Cost = cost;
            Count = count;
            ImagePath = imagePath;
        }

        public Drink()
        {

        }
    }
}
