using Microsoft.EntityFrameworkCore;
using VendingMachine.Server.Models;
using VendingMachine.Server.Models.Coin;

namespace VendingMachine.Server
{
    public class AppDBContext : DbContext
    {
        public DbSet<Coin> Coins { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coin>().HasData(
                    new Coin(1, 10),
                    new Coin(2, 10),
                    new Coin(5, 10),
                    new Coin(10, 10)
            );

            modelBuilder.Entity<Drink>().HasData(
                    new Drink("Water", 10, 10, "./Images/Water.jpg"),
                    new Drink("Juice", 20, 10, "./Images/Juice.jpg")
            );
        }
    }
}
