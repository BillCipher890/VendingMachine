using Microsoft.EntityFrameworkCore;
using VendingMachine.Server.Models;

namespace VendingMachine.Server.Repositories
{
    public class DrinkRepository : BaseRepository<Drink>, IDrinkRepository
    {
        private readonly DbSet<Drink> _dbSet;

        public DrinkRepository(AppDBContext context) : base(context)
        {
            _dbSet = context.Set<Drink>();
        }

        public async Task<Drink> GetAsync(string guid)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.Guid.Equals(guid));
        }
    }
}
