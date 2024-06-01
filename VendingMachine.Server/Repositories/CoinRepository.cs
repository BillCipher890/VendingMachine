using Microsoft.EntityFrameworkCore;
using VendingMachine.Server.Models.Coin;

namespace VendingMachine.Server.Repositories
{
    public class CoinRepository : BaseRepository<Coin>, ICoinRepository
    {
        private readonly DbSet<Coin> _dbSet;

        public CoinRepository(AppDBContext context) : base(context)
        {
            _dbSet = context.Set<Coin>();
        }
    }
}
