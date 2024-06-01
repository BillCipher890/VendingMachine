using VendingMachine.Server.Models;

namespace VendingMachine.Server.Repositories
{
    public interface IDrinkRepository : IRepository<Drink>
    {
        Task<Drink> GetAsync(string guid);
    }
}
