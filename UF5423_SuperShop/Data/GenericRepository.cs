using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity // Design pattern that serves as intermediary class between the controller and database to prevent malfunction when altering or migrating database.
    {
        private readonly DataContext _context; // Replaced by entity data context to be used in methods below.

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            //return _context.Set<T>().AsNoTracking().OrderBy(e => e.Id); // Order common property values. Define in specific entity controller for unique case.
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking() // 'AsNoTracking': disconnect from entities after completion.
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(T entity) // Bypass method.
        {
            await _context.Set<T>().AddAsync(entity); // Intermediary connection to keep entity in memory before saving to database.
            await SaveAllAsync(); // Add to database.
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; // Only connect to database if at least one change was successful, otherwise keep all in memory.
        }
    }
}
