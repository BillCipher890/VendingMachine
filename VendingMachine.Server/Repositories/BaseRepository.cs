using Microsoft.EntityFrameworkCore;

namespace VendingMachine.Server.Repositories
{
    public class BaseRepository<T> : IDisposable, IRepository<T> where T : class
    {
        private AppDBContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task AddListAsync(List<T> entities)
        {
            foreach (var item in entities.Distinct())
            {
                _dbSet.Add(item);
            }
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAllAsync()
        {

            IEnumerable<T> objects = _dbSet.AsEnumerable();

            foreach (T obj in objects)
            {
                _dbSet.Remove(obj);
            }

            await _context.SaveChangesAsync();

        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
