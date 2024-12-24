using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public interface IGenericRepository<T> where T : class // Generic entity repository interface. // Does not require instantiation when inherited by other interfaces.
    {
        IQueryable<T> GetAll(); // List all objects of current entity (T).

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistsAsync(int id);

        //Task<bool> SaveAllAsync(); // Defined in respective entity class.
    }
}
